using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using MyShop_Backend.DTO;
using MyShop_Backend.Enumerations;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Library;
using MyShop_Backend.Models;
using MyShop_Backend.ModelView;
using MyShop_Backend.Repositories.CartItemRepositories;
using MyShop_Backend.Repositories.OrderDetailRepositories;
using MyShop_Backend.Repositories.OrderRepositories;
using MyShop_Backend.Repositories.PaymentMethodRepositories;
using MyShop_Backend.Repositories.ProductReviewRepositories;
using MyShop_Backend.Repositories.ProductSizeRepositories;
using MyShop_Backend.Repositories.TransactionRepositories;
using MyShop_Backend.Request;
using MyShop_Backend.Response;
using MyShop_Backend.Services.CachingServices;
using MyShop_Backend.Services.Payments;
using MyShop_Backend.Storages;
using MyStore.Repository.ProductRepository;
using System.Linq.Expressions;

namespace MyShop_Backend.Services.Orders
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly ICartItemRepository _cartItemRepository;
		private readonly IOrderDetailRepository _orderDetailRepository;
		private readonly IProductSizeRepository _productSizeRepository;
		private readonly IProductRepository _productRepository;
		private readonly IProductReviewRepository _productReviewRepository;
		private readonly IPaymentMethodRepository _paymentMethodRepository;
		private readonly IPaymentService _paymentService;
		private readonly IVNPayLibrary _vnPayLibrary;
		private readonly ITransactionRepository _transaction;
		private readonly IPaymentMethodRepository _methodRepository;
		private readonly IFileStorage _fileStorage;
		private readonly IConfiguration _configuration;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly ICachingService _cache;
		private readonly IMapper _mapper;
		private readonly string pathReviewImages = "assets/images/reviews";
		public OrderService(IOrderRepository orderRepository,
			ICartItemRepository cartItemRepository,
			IOrderDetailRepository orderDetailRepository,
			IProductSizeRepository productSizeRepository,
			IProductRepository productRepository,
			IProductReviewRepository productReviewRepository,
			IPaymentMethodRepository paymentMethodRepository,
			IPaymentService paymentService,
			IVNPayLibrary vnPayLibrary,
			ITransactionRepository transaction,
			IPaymentMethodRepository methodRepository,
			IFileStorage fileStorage,
			IConfiguration configuration,
			IServiceScopeFactory serviceScopeFactory,
			ICachingService cache,
			IMapper mapper)
		{
			_orderRepository = orderRepository;
			_cartItemRepository = cartItemRepository;
			_orderDetailRepository = orderDetailRepository;
			_productSizeRepository = productSizeRepository;
			_productRepository = productRepository;
			_productReviewRepository = productReviewRepository;
			_paymentMethodRepository = paymentMethodRepository;
			_paymentService = paymentService;
			_vnPayLibrary = vnPayLibrary;
			_transaction = transaction;
			_methodRepository = methodRepository;
			_fileStorage = fileStorage;
			_configuration = configuration;
			_serviceScopeFactory = serviceScopeFactory;
			_cache = cache;
			_mapper = mapper;
		}
		struct OrderCache
		{
			public string Url { get; set; }
			public long OrderId { get; set; }
			public string? vnp_IpAddr { get; set; }
			public string? vnp_CreateDate { get; set; }
			public string? vnp_OrderInfo { get; set; }
		}
		public async Task<string?> CreateOrder(string userId, OrderRequest request)
		{
			using var transaction = await _transaction.BeginTransactionAsync();
			try
			{
				var now = DateTime.Now;
				var order = new Order
				{
					UserId = userId,
					Receiver = request.Receiver,
					DeliveryAddress = request.DeliveryAddress,
					District_Id = request.District_Id,
					Ward_Id = request.Ward_Id,
					OrderDate = now,
					Total = request.Total,	
				};

				var method = await _paymentMethodRepository
					.SingleOrDefaultAsync(x => x.Id == request.PaymentMethodId && x.IsActive)
					?? throw new ArgumentException(ErrorMessage.NOT_FOUND + "phương thức thanh toán");

				order.PaymentMethodId = method.Id;
				order.PaymentMethodName = method.Name;


				await _orderRepository.AddAsync(order);

				double total = 0;

				var cartItems = await _cartItemRepository.GetAsync(e => e.UserId == userId && request.CartIds.Contains(e.Id));

				var listpSizeUpdate = new List<ProductSize>();
				var listProductUpdate = new List<Product>();
				var listDetails = new List<OrderDetail>();

				foreach (var cartItem in cartItems)
				{
					var size = await _productSizeRepository
						.SingleAsyncInclude(e => e.ProductColorId == cartItem.ColorId && e.SizeId == cartItem.SizeId);

					if (size.InStock < cartItem.Quantity)
					{
						throw new Exception(ErrorMessage.SOLDOUT);
					}

					double price = cartItem.Product.Price - cartItem.Product.Price * (cartItem.Product.Discount / 100.0);

					price *= cartItem.Quantity;
					total += price;

					cartItem.Product.Sold += cartItem.Quantity;
					listProductUpdate.Add(cartItem.Product);

					size.InStock -= cartItem.Quantity;
					listpSizeUpdate.Add(size);

					listDetails.Add(new OrderDetail
					{
						OrderId = order.Id,
						ProductId = cartItem.ProductId,
						SizeName = size.Size.Name,
						SizeId = size.SizeId,
						Quantity = cartItem.Quantity,
						ColorName = size.ProductColor.ColorName,
						ColorId = size.ProductColorId,
						ProductName = cartItem.Product.Name,
						OriginPrice = cartItem.Product.Price,
						Price = price,
						ImageUrl = size.ProductColor.ImageUrl,
					});
				}

				double shipCost = CalcShip(total);
				order.ShippingCost = shipCost;

				total = total + shipCost;

				if (total != request.Total)
				{
					throw new Exception(ErrorMessage.BAD_REQUEST);
				}

				await _productSizeRepository.UpdateAsync(listpSizeUpdate);
				await _productRepository.UpdateAsync(listProductUpdate);
				await _orderDetailRepository.AddAsync(listDetails);
				await _cartItemRepository.DeleteRangeAsync(cartItems);

				string? paymentUrl = null;

				if (method.Name == PaymentMethodEnum.VNPay.ToString())
				{
					var orderInfo = new VNPayOrderInfo
					{
						OrderId = order.Id,
						Amount = total,
						CreatedDate = order.OrderDate,
						Status = order.OrderStatus?.ToString() ?? "0",
						OrderDesc = "Thanh toán đơn hàng: " + order.Id,
					};
					var userIP = "127.0.0.1";

					paymentUrl = _paymentService.GetVNPayURL(orderInfo, userIP);

					var orderCache = new OrderCache()
					{
						OrderId = order.Id,
						Url = paymentUrl,
						vnp_CreateDate = order.OrderDate.ToString("yyyyMMddHHmmss"),
						vnp_IpAddr = userIP,
						vnp_OrderInfo = orderInfo.OrderDesc,
					};

					var cacheOptions = new MemoryCacheEntryOptions
					{
						AbsoluteExpiration = DateTime.Now.AddMinutes(15)
					};
					cacheOptions.RegisterPostEvictionCallback(OnVNPayDeadline, this);
					_cache.Set("Order " + order.Id, orderCache, cacheOptions);
				}
				await transaction.CommitAsync();
				return paymentUrl;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				throw new Exception(ex.Message);
			}
		}

		public async Task DeleteOrder(long id)
		{
			var order = await _orderRepository.SingleOrDefaultAsync(e => e.Id == id);
			if (order != null)
			{
				await _orderRepository.DeleteAsync(order);
			}
			else throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);
		}

		public async Task<PagedResponse<OrderDTO>> GetAll(int page, int pageSize, string? keySearch)
		{
			int totalOrder;
			IEnumerable<Order> orders;
			if (string.IsNullOrEmpty(keySearch))
			{
				totalOrder = await _orderRepository.CountAsync();
				orders = await _orderRepository.GetPagedOrderByDescendingAsync(page, pageSize, null, e => e.CreatedAt);
			}
			else
			{
				bool isLong = long.TryParse(keySearch, out long idSearch);

				Expression<Func<Order, bool>> expression =
				e => e.Id.Equals(idSearch)
					|| (!isLong && e.PaymentMethodName != null && e.PaymentMethodName.Contains(keySearch));


				totalOrder = await _orderRepository.CountAsync(expression);
				orders = await _orderRepository.GetPagedOrderByDescendingAsync(page, pageSize, expression, e => e.CreatedAt);
			}
			var items = _mapper.Map<IEnumerable<OrderDTO>>(orders);

			return new PagedResponse<OrderDTO>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = totalOrder
			};
		}

		public async Task<OrderDetailsResponse> GetOrderDetail(long orderId, string userId)
		{
			var order = await _orderRepository.SingleOrDefaultAsyncInclude(e => e.Id == orderId && userId == e.UserId);
			if (order != null)
			{
				return _mapper.Map<OrderDetailsResponse>(order);
			}
			else throw new InvalidOperationException(ErrorMessage.ORDER_NOT_FOUND);
		}

		public async Task<OrderDetailsResponse> GetOrderDetail(long orderId)
		{
			var order = await _orderRepository.SingleOrDefaultAsyncInclude(e => e.Id == orderId);
			if (order != null)
			{
				return _mapper.Map<OrderDetailsResponse>(order);

			}
			else throw new InvalidOperationException(ErrorMessage.ORDER_NOT_FOUND);
		}

		public async Task<PagedResponse<OrderDTO>> GetOrdersByUserId(string userId, PageRequest page)
		{
			var orders = await _orderRepository.GetPagedOrderByDescendingAsync(page.Page, page.PageSize, e => e.UserId == userId, x => x.CreatedAt);
			var total = await _orderRepository.CountAsync(e => e.UserId == userId);
			var items = _mapper.Map<IEnumerable<OrderDTO>>(orders);

			foreach (var item in items)
			{
				var p = (await _orderDetailRepository.GetAsync(x => x.OrderId == item.Id)).FirstOrDefault();
				item.Product = _mapper.Map<ProductDTO>(p);
			}

			return new PagedResponse<OrderDTO>
			{
				Items = items,
				TotalItems = total,
				Page = page.Page,
				PageSize = page.PageSize,
			};
		}

		public async Task CancelOrder(long orderId, string userId)
		{
			var order = await _orderRepository.SingleOrDefaultAsync(e => e.Id == orderId && e.UserId == userId);
			if (order != null)
			{
				if (order.OrderStatus.Equals(DeliveryStatusEnum.Processing)
					|| order.OrderStatus.Equals(DeliveryStatusEnum.Confirmed))
				{
					order.OrderStatus = DeliveryStatusEnum.Canceled;

					_cache.Remove("Order " + orderId);
					await _orderRepository.UpdateAsync(order);
				}
				else throw new Exception(ErrorMessage.CANNOT_CANCEL);
			}
			else throw new ArgumentException($"Id {orderId} " + ErrorMessage.NOT_FOUND);
		}

		private async void OnVNPayDeadline(object key, object? value, EvictionReason reason, object? state)
		{
			if (value != null)
			{
				using var _scope = _serviceScopeFactory.CreateScope();
				var orderRepository = _scope.ServiceProvider.GetRequiredService<IOrderRepository>();
				var vnPayLibrary = _scope.ServiceProvider.GetRequiredService<IVNPayLibrary>();
				var configuration = _scope.ServiceProvider.GetRequiredService<IConfiguration>();


				var data = (OrderCache)value;
				var vnp_QueryDrUrl = configuration["VNPay:vnp_QueryDrUrl"] ?? throw new Exception(ErrorMessage.ERROR);
				var vnp_HashSecret = configuration["VNPay:vnp_HashSecret"] ?? throw new Exception(ErrorMessage.ERROR);
				var vnp_TmnCode = configuration["VNPay:vnp_TmnCode"] ?? throw new Exception(ErrorMessage.ERROR);

				var queryDr = new VNPayQueryDr
				{
					vnp_Command = "querydr",
					vnp_RequestId = data.OrderId.ToString(),
					vnp_Version = _vnPayLibrary.VERSION,
					vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
					vnp_TransactionDate = data.vnp_CreateDate,
					vnp_IpAddr = data.vnp_IpAddr,
					vnp_OrderInfo = data.vnp_OrderInfo,
					vnp_TmnCode = vnp_TmnCode,
					vnp_TxnRef = data.OrderId.ToString()
				};
				var checksum = vnPayLibrary.CreateSecureHashQueryDr(queryDr, vnp_HashSecret);

				var queryDrWithHash = new
				{
					queryDr.vnp_Command,
					queryDr.vnp_RequestId,
					queryDr.vnp_Version,
					queryDr.vnp_CreateDate,
					queryDr.vnp_TransactionDate,
					queryDr.vnp_IpAddr,
					queryDr.vnp_OrderInfo,
					queryDr.vnp_TmnCode,
					queryDr.vnp_TxnRef,
					vnp_SecureHash = checksum
				};

				using var httpClient = new HttpClient();

				var res = await httpClient.PostAsJsonAsync(vnp_QueryDrUrl, queryDrWithHash);
				VNPayQueryDrResponse? queryDrResponse = await res.Content.ReadFromJsonAsync<VNPayQueryDrResponse?>();

				if (queryDrResponse != null)
				{
					bool checkSignature = vnPayLibrary
						.ValidateQueryDrSignature(queryDrResponse, queryDrResponse.vnp_SecureHash, vnp_HashSecret);
					if (checkSignature && queryDrResponse.vnp_ResponseCode == "00")
					{
						var order = await orderRepository.FindAsync(data.OrderId);
						if (order != null)
						{
							long vnp_Amount = Convert.ToInt64(queryDrResponse.vnp_Amount) / 100;

							if (queryDrResponse.vnp_TransactionStatus == "00" && vnp_Amount == order.Total)
							{
								order.PaymentTranId = queryDrResponse.vnp_TransactionNo;
								order.AmountPaid = vnp_Amount;
								order.OrderStatus = DeliveryStatusEnum.Confirmed;
							}
							else
							{
								order.OrderStatus = DeliveryStatusEnum.Canceled;
							}
							await orderRepository.UpdateAsync(order);
						}
					}
				}
			}
		}
		public double CalcShip(double price) => price >= 500000 ? 0 : price >= 200000 ? 20000 : 30000;
		

		public async Task<OrderDTO> UpdateOrder(long id, string userId, UpdateOrderRequest request)
		{
			var order = await _orderRepository.SingleOrDefaultAsync(e => e.Id == id && e.UserId == userId);
			if (order != null && order.OrderStatus.Equals(DeliveryStatusEnum.Processing))
			{
				if (request.DeliveryAddress != null)
				{
					order.DeliveryAddress = request.DeliveryAddress;
				}
				if (request.ReceiverInfo != null)
				{
					order.DeliveryAddress = request.ReceiverInfo;
				}

				await _orderRepository.UpdateAsync(order);
				return _mapper.Map<OrderDTO>(order);
			}
			else throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);



		}

		public async Task<OrderDTO> UpdateOrder(long id, UpdateStatusOrderRequest request)
		{
			var order = await _orderRepository.SingleOrDefaultAsync(e => e.Id == id);
			if (order != null)
			{

				if (request.OrderStatus != null)
				{
					order.OrderStatus = request.OrderStatus;
				}

				await _orderRepository.UpdateAsync(order);
				return _mapper.Map<OrderDTO>(order);
			}
			else throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);
		}

		
		public async Task CancelOrder(long orderId)
		{
			var order = await _orderRepository.SingleOrDefaultAsync(e => e.Id == orderId);
			if (order != null)
			{
				if (order.OrderStatus.Equals(DeliveryStatusEnum.Processing)
					|| order.OrderStatus.Equals(DeliveryStatusEnum.Confirmed))
				{
					order.OrderStatus = DeliveryStatusEnum.Canceled;
					_cache.Remove("Order " + orderId);
					await _orderRepository.UpdateAsync(order);
				}
				else throw new InvalidDataException(ErrorMessage.CANNOT_CANCEL);
			}
			else throw new InvalidOperationException(ErrorMessage.ORDER_NOT_FOUND);
		}

		public async Task NextOrderStatus(long orderId)
		{
			var order = await _orderRepository.FindAsync(orderId);
			if (order != null)
			{
				if (!order.OrderStatus.Equals(DeliveryStatusEnum.Received) || !order.OrderStatus.Equals(DeliveryStatusEnum.Canceled))
				{
					order.OrderStatus += 1;
					await _orderRepository.UpdateAsync(order);
				}
				else throw new InvalidDataException(ErrorMessage.BAD_REQUEST);
			}
			else throw new InvalidOperationException(ErrorMessage.ORDER_NOT_FOUND);
		}

		public async Task OrderToShipping(long orderId, OrderToShippingRequest request)
		{
			var order = await _orderRepository.SingleOrDefaultAsyncInclude(e => e.Id == orderId)
			   ?? throw new InvalidOperationException(ErrorMessage.ORDER_NOT_FOUND);

			if (order.OrderStatus != DeliveryStatusEnum.Confirmed)
			{
				throw new InvalidDataException(ErrorMessage.BAD_REQUEST + ".Xem lại trạng thái đơn đặt hàng");
			}

			var token = _configuration["GHN:Token"];
			var shopId = _configuration["GHN:ShopId"];
			var url = _configuration["GHN:Url"] + "/create";

			var receiver = order.Receiver.Split("-").Select(e => e?.Trim()).ToArray();
			var to_name = receiver[0];
			var to_phone = receiver[1];


			if (token == null || shopId == null || url == null
				|| to_name == null || to_phone == null)
			{
				throw new ArgumentNullException(ErrorMessage.ARGUMENT_NULL);
			}

			var to_address = order.DeliveryAddress;
			var to_ward_code = order.Ward_Id;
			var to_district_id = order.District_Id;

			var items = order.OrderDetails.Select(e => new
			{
				name = e.ProductName,
				quantity = e.Quantity,
				price = (int)Math.Floor(e.Price)
			}).ToArray();

			var cod_amount = order.AmountPaid < order.Total ? order.Total : 0;

			var data = new
			{
				cod_amount,
				to_name,
				to_phone,
				to_address,
				to_ward_code,
				to_district_id,
				service_type_id = 2,
				payment_type_id = 1,
				weight = request.Weight,
				length = request.Length,
				width = request.Width,
				height = request.Height,
				required_note = request.RequiredNote.ToString(),
				items,
			};

			using var httpClient = new HttpClient();

			httpClient.DefaultRequestHeaders.Add("ShopId", shopId);
			httpClient.DefaultRequestHeaders.Add("Token", token);

			var res = await httpClient.PostAsJsonAsync(url, data);
			var dataResponse = await res.Content.ReadFromJsonAsync<GHNResponse>();
			if (!res.IsSuccessStatusCode)
			{
				throw new InvalidDataException(dataResponse?.Message ?? ErrorMessage.BAD_REQUEST);
			}

			order.ShippingCode = dataResponse?.Data?.Order_code;
			order.Expected_delivery_time = dataResponse?.Data?.Expected_delivery_time;

			order.OrderStatus = DeliveryStatusEnum.Shipping;
			await _orderRepository.UpdateAsync(order);
		}

		public async Task Review(long orderId, string userId, IEnumerable<ReviewRequest> reviews)
		{
			using var transaction = await _transaction.BeginTransactionAsync();
			try
			{
				var order = await _orderRepository.SingleOrDefaultAsyncInclude(e => e.Id == orderId && e.UserId == userId) ?? throw new InvalidOperationException(ErrorMessage.NOT_FOUND);

				if (order.OrderStatus != DeliveryStatusEnum.Received)
				{
					throw new InvalidOperationException("Chưa thể đánh giá đơn hàng này.");
				}

				List<ProductReview> pReviews = new();
				List<Product> products = new();

				foreach (var review in reviews)
				{
					var productPath = pathReviewImages + "/" + review.ProductId;
					List<string>? pathNames = null;

					if (review.Images != null)
					{
						pathNames = new();
						var imgNames = review.Images.Select(image =>
						{
							var name = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
							pathNames.Add(Path.Combine(productPath, name));
							return name;
						}).ToList();
						await _fileStorage.SaveAsync(productPath, review.Images, imgNames);
					}

					var product = await _productRepository.FindAsync(review.ProductId);
					if (product != null)
					{
						var currentStar = product.Rating * product.RatingCount;
						product.Rating = (currentStar + review.Star) / (product.RatingCount + 1);
						product.RatingCount += 1;

						var orderDetails = order.OrderDetails.SingleOrDefault(x => x.ProductId == review.ProductId && x.ColorName == review.ColorName && x.SizeName == review.SizeName);
						var colorName = orderDetails?.ColorName ?? "";
						var sizeName = orderDetails?.SizeName ?? "";
						products.Add(product);
						pReviews.Add(new ProductReview
						{

							ProductId = review.ProductId,
							UserId = userId,
							Star = review.Star,
							Description = review.Description,
							ImagesUrls = pathNames,
							ColorName = colorName,
							SizeName = sizeName,
							Enable = true,
						});
					}
				}

				await _productReviewRepository.AddAsync(pReviews);
				await _productRepository.UpdateAsync(products);
				order.Reviewed = true;
				await _orderRepository.UpdateAsync(order);
				await transaction.CommitAsync();
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public async Task<PagedResponse<OrderDTO>> GetWithOrderStatus(DeliveryStatusEnum statusEnum, PageRequest request)
		{
			int totalOrder;
			IEnumerable<Order> orders;

			int page = request.Page, pageSize = request.PageSize;
			string? key = request.Key?.ToLower();

			Expression<Func<Order, DateTime?>> sortExpression = e => e.UpdatedAt;

			if (statusEnum == DeliveryStatusEnum.Processing)
			{
				sortExpression = e => e.CreatedAt;
			}
			if (string.IsNullOrEmpty(key))
			{
				totalOrder = await _orderRepository.CountAsync(e => e.OrderStatus == statusEnum);
				orders = await _orderRepository.GetPagedOrderByDescendingAsync(page, pageSize, e => e.OrderStatus == statusEnum, sortExpression);
			}
			else
			{
				bool isLong = long.TryParse(key, out long idSearch);

				Expression<Func<Order, bool>> expression =
					e => e.OrderStatus == statusEnum &&
					(isLong && e.Id.Equals(idSearch) ||
					e.PaymentMethodName.ToLower().Contains(key));

				totalOrder = await _orderRepository.CountAsync(expression);
				orders = await _orderRepository.GetPagedOrderByDescendingAsync(page, pageSize, expression, sortExpression);
			}

			var items = _mapper.Map<IEnumerable<OrderDTO>>(orders);
			return new PagedResponse<OrderDTO>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = totalOrder
			};


		}

		public async Task<PagedResponse<OrderDTO>> GetWithOrderStatusUser(string userId, DeliveryStatusEnum statusEnum, PageRequest request)
		{
			int totalOrder;
			IEnumerable<Order> orders;

			int page = request.Page, pageSize = request.PageSize;
			string? key = request.Key?.ToLower();

			Expression<Func<Order, DateTime?>> sortExpression = e => e.UpdatedAt;

			if (statusEnum == DeliveryStatusEnum.Processing)
			{
				sortExpression = e => e.CreatedAt;
			}

			if (string.IsNullOrEmpty(key))
			{
				totalOrder = await _orderRepository.CountAsync(e => e.UserId == userId && e.OrderStatus == statusEnum);
				orders = await _orderRepository.GetPagedOrderByDescendingAsync(page, pageSize, x => x.UserId == userId && x.OrderStatus == statusEnum, sortExpression);
			}

			else
			{
				bool isLong = long.TryParse(key, out long idSearch);

				Expression<Func<Order, bool>> expression =
					e => e.OrderStatus == statusEnum &&
					e.UserId == userId &&
					(isLong && e.Id.Equals(idSearch) ||
					e.PaymentMethodName.ToLower().Contains(key));

				totalOrder = await _orderRepository.CountAsync(expression);
				orders = await _orderRepository.GetPagedOrderByDescendingAsync(page, pageSize, expression, sortExpression);
			}

			var items = _mapper.Map<IEnumerable<OrderDTO>>(orders);
			foreach (var item in items)
			{
				var p = (await _orderDetailRepository.GetAsync(x => x.OrderId == item.Id)).FirstOrDefault()?.Product;
				item.Product = _mapper.Map<ProductDTO>(p);
			}
			return new PagedResponse<OrderDTO>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = totalOrder
			};
		}

		public async Task ReceivedOrder(long orderId, string userId, UpdateStatusOrderRequest request)
		{
			var now = DateTime.Now;
			var order = await _orderRepository.SingleOrDefaultAsync(e => e.Id == orderId && e.UserId == userId);
				if (order != null)
				{
					if (order.OrderStatus.Equals(DeliveryStatusEnum.Shipping))
					{
						order.OrderStatus = DeliveryStatusEnum.Received;
						order.ReceivedDate = now;
					await _orderRepository.UpdateAsync(order);
					}
					else throw new Exception(ErrorMessage.CANNOT_RECEIVED);
				}
				else throw new ArgumentException($"Id {orderId} " + ErrorMessage.NOT_FOUND);
		
		}
	}
}
