using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.Enumerations;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.ImageRepositories;
using MyShop_Backend.Repositories.ProductColorRepositories;
using MyShop_Backend.Repositories.ProductSizeRepositories;
using MyShop_Backend.Repositories.TransactionRepositories;
using MyShop_Backend.Request;
using MyShop_Backend.Response;
using MyShop_Backend.Storages;
using MyStore.Repository.ProductRepository;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Text;
using MyShop_Backend.Repositories.ProductReviewRepositories;

namespace MyShop_Backend.Services.Products
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IProductSizeRepository _productSizeRepository;
		private readonly IImageRepository _imageRepository;
		private readonly IMapper _mapper;
		private readonly IFileStorage _fileStorage;
		private readonly string path = "assets/images/products";
		private readonly IProductColorRepository _productColorRepository;
		private readonly IProductReviewRepository _productReviewRepository;
		private readonly ITransactionRepository _transactionRepository;

		public ProductService(IProductRepository productRepository, IProductSizeRepository productSizeRepository,
			IProductColorRepository productColorRepository, IProductReviewRepository productReviewRepository, IImageRepository imageRepository, ITransactionRepository transactionRepository, IMapper mapper, IFileStorage fileStorage)
		{
			_productRepository = productRepository;
			_productSizeRepository = productSizeRepository;
			_productColorRepository = productColorRepository;
			_productReviewRepository = productReviewRepository;
			_transactionRepository = transactionRepository;
			_imageRepository = imageRepository;
			_mapper = mapper;
			_fileStorage = fileStorage;
		}

		public async Task<ProductDTO> CreateProductAsync(ProductRequest request, IFormFileCollection images)
		{
			using var transaction = await _transactionRepository.BeginTransactionAsync();
			try
			{
				var product = _mapper.Map<Product>(request);
				await _productRepository.AddAsync(product);
				var productPath = path + "/" + product.Id;

				List<string> colorFileNames = new();
				List<IFormFile> colorImages = new();
				List<ProductSize> productSizes = new();

				foreach (var color in request.ColorSizes.ToHashSet())
				{
					var name = "";
					if (color.Image != null)
					{
						name = Guid.NewGuid().ToString() + Path.GetExtension(color.Image.FileName);
						colorFileNames.Add(name);
						colorImages.Add(color.Image);
					}

					var productColor = new ProductColor
					{
						ColorName = color.ColorName,
						ProductId = product.Id,
						ImageUrl = Path.Combine(productPath, name)
					};

					await _productColorRepository.AddAsync(productColor);

					var sizes = color.SizeInStocks.Select(size =>
					{
						return new ProductSize
						{
							ProductColorId = productColor.Id,
							SizeId = size.SizeId,
							InStock = size.InStock,
						};
					});
					productSizes.AddRange(sizes);
				}
				await _productSizeRepository.AddAsync(productSizes);


				List<string> commonFileNames = new();
				var imgs = images.Select(file =>
				{
					var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					commonFileNames.Add(name);
					var image = new Image()
					{
						ProductId = product.Id,
						ImageUrl = Path.Combine(productPath, name),
					};
					return image;
				});
				await _imageRepository.AddAsync(imgs);

				colorImages.AddRange(images);
				colorFileNames.AddRange(commonFileNames);

				await _fileStorage.SaveAsync(productPath, colorImages, colorFileNames);

				await transaction.CommitAsync();

				var res = _mapper.Map<ProductDTO>(product);

				var image = imgs.FirstOrDefault();
				if (image != null)
				{
					res.ImageUrl = image.ImageUrl; ;
				}
				return res;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task DeleteProductAsync(long id)
		{
			var product = await _productRepository.FindAsync(id);
			if (product != null)
			{
				var images = await _imageRepository.GetImageByProductIdAsync(id);
				var colorImages = await _productColorRepository.GetAsync(e => e.ProductId == id);

				var deleteList = colorImages.Select(e => e.ImageUrl).ToList();
				deleteList.AddRange(images.Select(e => e.ImageUrl));

				_fileStorage.Delete(deleteList);
				await _productRepository.DeleteAsync(product);
			}
			else throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);
		}

		public async Task<PagedResponse<ProductDTO>> GetFilterProductsAsync(ProductFiltersRequest filters)
		{
			try
			{
				int totalProduct = 0;
				IEnumerable<Product> products = [];
				Expression<Func<Product, bool>> expression = e => e.Enable;

				if (filters.MinPrice != null && filters.MaxPrice != null)
				{
					expression = CombineExpressions(expression, e =>
						(e.Price - (e.Price * (e.Discount / 100.0))) >= filters.MinPrice
						&&
						(e.Price - (e.Price * (e.Discount / 100.0))) <= filters.MaxPrice
					);
				}
				else
				{
					if (filters.MinPrice != null)
					{
						expression = CombineExpressions(expression, e => (e.Price - (e.Price * (e.Discount / 100.0))) >= filters.MinPrice);
					}
					else if (filters.MaxPrice != null)
					{
						expression = CombineExpressions(expression, e => (e.Price - (e.Price * (e.Discount / 100.0))) <= filters.MaxPrice);
					}
				}

				if (!string.IsNullOrEmpty(filters.Key))
				{
					var inputWords = filters.Key.Trim().Split(' ').Select(word => word.ToLower());

					expression = CombineExpressions(expression, e => inputWords.All(word => e.Name.ToLower().Contains(word)));
				}

				if (filters.Discount != null && filters.Discount == true)
				{
					expression = CombineExpressions(expression, e => e.Discount > 0);
				}
				//if (filters.Rating != null)
				//{
				//	expression = CombineExpressions(expression, e => e.ProductReviews.Average(e => e.Star) >= filters.Rating);
				//}
				if (filters.CategoryIds != null && filters.CategoryIds.Any())
				{
					expression = CombineExpressions(expression, e => filters.CategoryIds.Contains(e.CategoryId));
				}
				if (filters.BrandIds != null && filters.BrandIds.Any())
				{
					expression = CombineExpressions(expression, e => filters.BrandIds.Contains(e.BrandId));
				}

				//if (filters.MaterialIds != null && filters.MaterialIds.Any())
				//{
				//	expression = CombineExpressions(expression,
				//		e => filters.MaterialIds.Any(id => e.Materials.Any(m => m.MaterialId == id)));
				//}

				//if (filters.Genders != null && filters.Genders.Any())
				//{
				//	expression = CombineExpressions(expression, e => filters.Genders.Contains(e.Gender));
				//}

				totalProduct = await _productRepository.CountAsync(expression);
				Expression<Func<Product, double>> priceExp = e => e.Price - (e.Price * (e.Discount / 100.0));

				 products = filters.Sorter switch
				{
					SortEnum.SOLD => await _productRepository
											   .GetPagedOrderByDescendingAsync(filters.Page, filters.PageSize, expression, e => e.Sold),
					SortEnum.PRICE_ASC => await _productRepository
												.GetPagedAsync(filters.Page, filters.PageSize, expression, priceExp),
					SortEnum.PRICE_DESC => await _productRepository
											   .GetPagedOrderByDescendingAsync(filters.Page, filters.PageSize, expression, priceExp),
					SortEnum.NEWEST => await _productRepository
											   .GetPagedOrderByDescendingAsync(filters.Page, filters.PageSize, expression, e => e.CreatedAt),
					_ => await _productRepository
											   .GetPagedOrderByDescendingAsync(filters.Page, filters.PageSize, expression, e => e.CreatedAt),
				};
				var res = _mapper.Map<IEnumerable<ProductDTO>>(products);
				//.Select(x =>
				//{
				//    var image = products.Single(e => e.Id == x.Id).Images.FirstOrDefault();
				//    if (image != null)
				//    {
				//        x.ImageUrl = image.ImageUrl;
				//    }
				//    return x;
				//});

				return new PagedResponse<ProductDTO>
				{
					Items = res,
					Page = filters.Page,
					PageSize = filters.PageSize,
					TotalItems = totalProduct
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
		{
			var parameter = expr1.Parameters[0];
			var body = Expression.AndAlso(expr1.Body, Expression.Invoke(expr2, parameter));
			return Expression.Lambda<Func<T, bool>>(body, parameter);
		}

		public async Task<PagedResponse<ProductDTO>> GetFeaturedProductsAsync(int page, int pageSize)
		{
			var products = await _productRepository.GetPagedOrderByDescendingAsync(page, pageSize, e => e.Enable, x => x.Price);

			var total = await _productRepository.CountAsync(e => e.Enable);

			var items = _mapper.Map<IEnumerable<ProductDTO>>(products);

			return new PagedResponse<ProductDTO>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = total
			};
		}

		public async Task<ProductDetailsResponse> GetProductAsync(long id)
		{
			var product = await _productRepository.SingleOrDefaultAsync(e => e.Id == id);
			if (product != null)
			{
				var res = _mapper.Map<ProductDetailsResponse>(product);

				res.ColorSizes = _mapper.Map<IEnumerable<ColorSizeResponse>>(product.ProductColors);
				
				res.ImageUrls = product.Images.Select(e => e.ImageUrl);

				return res;
			}
			else throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);
		}

		public async Task<PagedResponse<ProductDTO>> GetAllProductsAsync(int page, int pageSize, string? keySearch)
		{
			try
			{
				int totalProduct;
				IEnumerable<Product> products;
				if (string.IsNullOrEmpty(keySearch))
				{
					totalProduct = await _productRepository.CountAsync();
					products = await _productRepository.GetPageProductAsync(page, pageSize);
				}
				else
				{
					Expression<Func<Product, bool>> expression = e =>
						e.Name.Contains(keySearch)
						|| e.Sold.ToString().Equals(keySearch)
						|| e.Price.ToString().Equals(keySearch);

					totalProduct = await _productRepository.CountAsync(expression);
					products = await _productRepository.GetPageProductAsync(page, pageSize, keySearch);
				}

				var res = _mapper.Map<IEnumerable<ProductDTO>>(products);
				foreach (var product in res)
				{
					var image = await _imageRepository.GetFirstImageByProductAsync(product.Id);
					if (image != null)
					{
						product.ImageUrl = image.ImageUrl;
					}
				}

				return new PagedResponse<ProductDTO>
				{
					Items = res,
					Page = page,
					PageSize = pageSize,
					TotalItems = totalProduct
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task<IEnumerable<ProductDTO>> GetSearchProducts(string key)
		{
			var inputWords = key.Trim().Split(' ').Select(word => word.ToLower());

			var products = await _productRepository.GetPagedAsync(1, 5,
				e => inputWords.All(word => e.Name.ToLower().Contains(word)), e => e.Name);

			if (!products.Any())
			{
				var productList = await _productRepository.GetPagedAsync(1, 20,
					e => inputWords.Any(word => e.Name.ToLower().Contains(word)), e => e.Name);

				products = productList.Where(e => IsMatchingSearchCriteriaWithoutTones(e.Name, inputWords)).Take(5);
			}
			return _mapper.Map<IEnumerable<ProductDTO>>(products);
		}

		private bool IsMatchingSearchCriteriaWithoutTones(string productName, IEnumerable<string> inputWords)
		{
			var normalizedProductName = RemoveVietnameseTones(productName).ToLower();
			return inputWords
				.Select(word => RemoveVietnameseTones(word.ToLower()))
				.All(normalizedProductName.Contains);
		}

		public static string RemoveVietnameseTones(string str)
		{
			string normalizedString = str.Normalize(NormalizationForm.FormD);

			var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string withoutTones = regex.Replace(normalizedString, string.Empty);

			return withoutTones.Replace('đ', 'd').Replace('Đ', 'D');
		}
		public async Task<ProductDTO> UpdateProductAsync(long id, ProductRequest request, IFormFileCollection images)
		{
			var product = await _productRepository.SingleOrDefaultAsync(e => e.Id == id);
			if (product != null)
			{
				using var transaction = await _transactionRepository.BeginTransactionAsync();
				try
				{
					product.Name = request.Name;
					product.Description = request.Description;
					product.Price = request.Price;
					product.CategoryId = request.CategoryId;
					product.BrandId = request.BrandId;
					product.Enable = request.Enable;
					product.Discount = request.Discount;

					var productPath = path + "/" + product.Id;

					List<string> colorFileNames = new();
					List<IFormFile> colorImages = new();
					List<ProductSize> productSizes = new();

					List<ProductColor> pColorDelete = new();
					var oldProductColors = await _productColorRepository.GetAsync(e => e.ProductId == product.Id);

					var oldColor = request.ColorSizes.Select(e => e.Id);
					if (oldColor == null || !oldColor.Any())
					{
						pColorDelete.AddRange(oldProductColors);
					}
					else
					{
						var colorDel = oldProductColors.Where(old => !request.ColorSizes.Select(e => e.Id).Contains(old.Id));
						pColorDelete.AddRange(colorDel);

						//cập nhật số lượng size cũ
						var oldIds = request.ColorSizes.Where(e => e.Id != null).Select(e => e.Id);
						var colorUpdate = oldProductColors.Where(old => oldIds.Contains(old.Id));

						foreach (var color in colorUpdate)
						{
							var matchingColor = request.ColorSizes.Single(e => e.Id == color.Id);
							foreach (var size in color.ProductSizes)
							{
								var matchingSize = matchingColor.SizeInStocks.Single(s => s.SizeId == size.SizeId);
								size.InStock = matchingSize.InStock;
							}
						}

					}
					//xóa màu
					_fileStorage.Delete(pColorDelete.Select(e => e.ImageUrl));
					await _productColorRepository.DeleteRangeAsync(pColorDelete);

					//thêm màu
					var newColorImage = request.ColorSizes.Where(e => e.Image != null && e.Id == null);
					if (newColorImage.Any())
					{
						foreach (var color in newColorImage)
						{
							var name = "";
							if (color.Image != null)
							{
								name = Guid.NewGuid().ToString() + Path.GetExtension(color.Image.FileName);
								colorFileNames.Add(name);
								colorImages.Add(color.Image);
							}

							var productColor = new ProductColor
							{
								ColorName = color.ColorName,
								ProductId = product.Id,
								ImageUrl = Path.Combine(productPath, name)
							};

							await _productColorRepository.AddAsync(productColor);

							var sizes = color.SizeInStocks.Select(size =>
							{
								return new ProductSize
								{
									ProductColorId = productColor.Id,
									SizeId = size.SizeId,
									InStock = size.InStock,
								};
							});
							productSizes.AddRange(sizes);
						}
						await _productSizeRepository.AddAsync(productSizes);
						await _fileStorage.SaveAsync(productPath, colorImages, colorFileNames);
					}

					//var pMaterials = await _productMaterialRepository.GetAsync(e => e.ProductId == product.Id);
					//await _productMaterialRepository.DeleteRangeAsync(pMaterials);

					//var productMaterials = request.MaterialIds.Select(e => new ProductMaterial
					//{
					//	ProductId = id,
					//	MaterialId = e
					//});
					//await _productMaterialRepository.AddAsync(productMaterials);

					List<Image> imageDelete = new();
					var oldImgs = await _imageRepository.GetImageByProductIdAsync(id);
					if (request.ImageUrls == null || !request.ImageUrls.Any())
					{
						imageDelete.AddRange(oldImgs);
					}
					else
					{
						var imgsToDelete = oldImgs.Where(old => !request.ImageUrls.Contains(old.ImageUrl));
						imageDelete.AddRange(imgsToDelete);
					}
					_fileStorage.Delete(imageDelete.Select(e => e.ImageUrl));
					await _imageRepository.DeleteRangeAsync(imageDelete);

					if (images.Count > 0)
					{
						List<string> fileNames = new();
						var imgs = images.Select(file =>
						{
							var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
							fileNames.Add(name);
							var image = new Image()
							{
								ProductId = id,
								ImageUrl = Path.Combine(productPath, name),
							};
							return image;
						});
						await _imageRepository.AddAsync(imgs);
						await _fileStorage.SaveAsync(productPath, images, fileNames);
					}

					await _productRepository.UpdateAsync(product);
					await transaction.CommitAsync();
					return _mapper.Map<ProductDTO>(product);
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					throw new Exception(ex.InnerException?.Message ?? ex.Message);
				}
			}
			else throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);
		}

		public async Task<bool> UpdateProductEnableAsync(long id, UpdateEnableRequest request)
		{
			var product = await _productRepository.FindAsync(id);
			if (product != null)
			{
				product.Enable = request.Enable;
				await _productRepository.UpdateAsync(product);
				return product.Enable;
			}
			else throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);
		}

		private string MaskUsername(string username)
		{
			var words = username.Split(" ");
			return string.Join(" ", words.Select(x =>
			{
				var trimmedWord = x.Trim();
				if (trimmedWord.Length > 1)
				{
					return $"{trimmedWord[0]}{new string('*', trimmedWord.Length - 1)}";
				}
				return trimmedWord;
			}));
		}
		public async Task<PagedResponse<ReviewDTO>> GetReviews(long id, PageRequest request)
		{
			var reviews = await _productReviewRepository
				.GetPagedOrderByDescendingAsync(request.Page, request.PageSize, e => e.ProductId == id, e => e.CreatedAt);

			var total = await _productReviewRepository.CountAsync(e => e.ProductId == id);

			var items = _mapper.Map<IEnumerable<ReviewDTO>>(reviews).Select(x =>
			{
				x.Username = MaskUsername(x.Username);
				return x;
			});

			return new PagedResponse<ReviewDTO>
			{
				Items = items,
				TotalItems = total,
				Page = request.Page,
				PageSize = request.PageSize,
			};
		}
	}
}