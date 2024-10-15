using AutoMapper;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.ModelView;
using MyShop_Backend.Repositories.CartItemRepositories;
using MyShop_Backend.Repositories.ImageRepositories;
using MyShop_Backend.Repositories.ProductColorRepositories;
using MyShop_Backend.Repositories.ProductSizeRepositories;
using MyShop_Backend.Repositories.SizeRepositories;
using MyShop_Backend.Repositories.TransactionRepositories;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Carts
{
	public class CartService : ICartService
	{
		private readonly ICartItemRepository _cartItemRepository;
		private readonly IProductColorRepository _productColorRepository;
		private readonly IProductSizeRepository _productSizeRepository;
		private readonly ISizeRepository _sizeRepository;
		private readonly IImageRepository _imageRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IMapper _mapper;

		public CartService(ICartItemRepository cartItemRepository, IProductColorRepository productColorRepository, IProductSizeRepository productSizeRepository, ISizeRepository sizeRepository, IImageRepository imageRepository, ITransactionRepository transactionRepository, IMapper mapper)
		{
			_cartItemRepository = cartItemRepository;
			_productColorRepository = productColorRepository;
			_productSizeRepository = productSizeRepository;
			_sizeRepository = sizeRepository;
			_imageRepository = imageRepository;
			_transactionRepository = transactionRepository;
			_mapper = mapper;
		}
		public async Task AddToCart(string userId, CartRequest request)
		{
			try
			{
				var size = await _productSizeRepository.SingleAsync(e => e.ProductColorId == request.ColorId && e.SizeId == request.SizeId);
				if (size.InStock <= 0)
				{
					throw new Exception(ErrorMessage.SOLDOUT);
				}

				var exist = await _cartItemRepository.SingleOrDefaultAsync(
					e => e.ProductId == request.ProductId &&
					e.UserId == userId &&
					e.SizeId == request.SizeId &&
					e.ColorId == request.ColorId);

				if (exist != null)
				{
					if ((request.Quantity + exist.Quantity) > size.InStock)
					{
						throw new Exception(ErrorMessage.CART_MAXIMUM);
					}

					exist.Quantity += request.Quantity;
					await _cartItemRepository.UpdateAsync(exist);
				}
				else
				{
					var item = new CartItem()
					{
						ProductId = request.ProductId,
						UserId = userId,
						Quantity = request.Quantity,
						SizeId = request.SizeId,
						ColorId = request.ColorId,
					};

					await _cartItemRepository.AddAsync(item);
				}

			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}

		public async Task<int> CountCart(string userId)
		{
			return await _cartItemRepository.CountAsync(e => e.UserId
			== userId);
		}

		public async Task DeleteCartAsync(string userId, IEnumerable<long> productId)
		{
			try
			{
				await _cartItemRepository.DeleteByCartId(userId, productId);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task<IEnumerable<CartItemResponse>> GetAllByUserId(string userId)
		{
			var items = await _cartItemRepository.GetAsync(e => e.UserId == userId);

			var res = items.Select(cartItem =>
			{
				var color = cartItem.Product.ProductColors.Single(x => x.Id == cartItem.ColorId);
				var size = color.ProductSizes.Single(x => x.SizeId == cartItem.SizeId);

				return new CartItemResponse
				{
					Id = cartItem.Id,
					ProductId = cartItem.ProductId,
					OriginPrice = cartItem.Product.Price,
					Discount = cartItem.Product.Discount,
					Quantity = cartItem.Quantity,
					ProductName = cartItem.Product.Name,
					ImageUrl = color?.ImageUrl,
					ColorId = cartItem.ColorId,
					SizeId = cartItem.SizeId,
					ColorName = color?.ColorName,
					InStock = size.InStock,
					SizeName = size.Size.Name,
					SizeInStocks = _mapper.Map<IEnumerable<SizeInStock>>(color?.ProductSizes ?? [])
				};
			});
			return res;
		}

		public async Task<CartItemResponse> UpdateCartItem(string cartId, string userId, UpdateCartItemRequest request)
		{
			try
			{
				var cartItem = await _cartItemRepository.SingleOrDefaultAsync(e => e.Id == cartId && e.UserId == userId);
				if (cartItem != null)
				{
					var color = cartItem.Product.ProductColors.Single(x => x.Id == cartItem.ColorId);
					var size = color.ProductSizes.Single(x => x.SizeId == request.SizeId);

					if (request.SizeId.HasValue)
					{
						cartItem.SizeId = request.SizeId.Value;
					}
					if (request.Quantity.HasValue)
					{
						if (size.InStock > 0 && request.Quantity.Value <= size.InStock)
						{
							cartItem.Quantity = request.Quantity.Value;
						}
						else throw new Exception(ErrorMessage.SOLDOUT);
					}
					await _cartItemRepository.UpdateAsync(cartItem);

					return new CartItemResponse
					{
						Id = cartItem.Id,
						ProductId = cartItem.ProductId,
						OriginPrice = cartItem.Product.Price,
						Discount = cartItem.Product.Discount,
						Quantity = cartItem.Quantity,
						ProductName = cartItem.Product.Name,
						ImageUrl = color?.ImageUrl,
						ColorId = cartItem.ColorId,
						SizeId = cartItem.SizeId,
						ColorName = color?.ColorName,
						InStock = size.InStock,
						SizeName = size.Size.Name,
						SizeInStocks = _mapper.Map<IEnumerable<SizeInStock>>(color?.ProductSizes ?? [])
					};
				}
				throw new ArgumentException(ErrorMessage.NOT_FOUND + " sản phẩm");
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
