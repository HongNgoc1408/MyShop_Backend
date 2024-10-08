using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MyShop_Backend.DTO;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.ImageRepositories;
using MyShop_Backend.Repositories.ProductRepositories;
using MyShop_Backend.Request;
using MyShop_Backend.Response;
using MyShop_Backend.Storages;

namespace MyShop_Backend.Services.ProductServices
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		private readonly IFileStorage _fileStorage;
		private readonly IImageRepository _imageRepository;

		private readonly string path = "assets/images/products";

		public ProductService(IProductRepository productRepository, IImageRepository imageRepository, IMapper mapper, IFileStorage fileStorage)
		{
			_productRepository = productRepository;
			_mapper = mapper;
			_fileStorage = fileStorage;
			_imageRepository = imageRepository;
		}

		public async Task<ProductDTO> AddProductAsync(ProductRequest request, IFormFileCollection images)
		{
			try
			{
				var product = _mapper.Map<Product>(request);

				await _productRepository.AddAsync(product);

				IList<string> fileNames = new List<string>();
				var imgs = images.Select(file =>
				{
					var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					fileNames.Add(name);
					var image = new Image()
					{
						ProductId = product.Id,
						ImageURL = Path.Combine(path, name)
					};
					return image;
				});

				await _imageRepository.AddAsync(imgs);
				await _fileStorage.SaveAsync(path, images, fileNames);

				var res = _mapper.Map<ProductDTO>(product);
				var image = await _imageRepository.GetFirstImageByProductAsync(product.Id);
				if (image != null)
				{
					res.ImageUrl = image.ImageURL;
				}
				return res;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public Task<ProductDTO> CreateProductAsync(ProductRequest request, IFormFileCollection images)
		{
			throw new NotImplementedException();
		}

		public async Task DeleteProductAsync(int id)
		{
			var product = await _productRepository.FindAsync(id);
			if (product != null)
			{
				var images = await _imageRepository.GetImageProductAsync(id);
				_fileStorage.Delete(images.Select(e => e.ImageURL));

				await _productRepository.DeleteAsync(product);
			}
			else
			{
				throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);
			}
		}

		public async Task<PagedResponse<ProductDTO>> GetAllProductAsync(int page, int pageSize, string? search)
		{
			try
			{
				int totalProduct;
				IEnumerable<Product> products;
				if (string.IsNullOrEmpty(search))
				{
					totalProduct = await _productRepository.CountAsync();
					products = await _productRepository.GetPageProductAsync(page, pageSize);
				}
				else
				{
					totalProduct = await _productRepository.CountAsync();
					products = await _productRepository.GetPageProductAsync(page, pageSize, search);
				}
				var res = _mapper.Map<IEnumerable<ProductDTO>>(products);
				foreach (var product in res)
				{
					var image = await _imageRepository.GetFirstImageByProductAsync(product.Id);
					if (image != null)
					{
						product.ImageUrl = image.ImageURL;
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

		public async Task<ProductDetailResponse> GetProductById(int id)
		{
			var product = await _productRepository.GetProductByIdAsync(id);
			if (product != null)
			{
				var res = _mapper.Map<ProductDetailResponse>(product);
				res.ImageURLs = product.Images.Select(x => x.ImageURL);
				return res;
			}
			else throw new ArgumentException($"Id {id}" + ErrorMessage.NOT_FOUND);
		}

		public Task<PagedResponse<ProductDTO>> GetProductsAsync(int page, int pageSize, string? keySearch)
		{
			throw new NotImplementedException();
		}

		public async Task<ProductDTO> UpdateProduct(int id, ProductRequest request, IFormFileCollection images)
		{
			var product = await _productRepository.FindAsync(id);
			if (product != null)
			{
				try
				{
					product.Name = request.Name;
					product.Price = request.Price;
					product.Quantity = request.Quantity;
					product.Sold = request.Sold;
					product.Discount = request.Discount;
					product.Description = request.Description;

					var oldImgs = await _imageRepository.GetImageProductAsync(id);
					List<Image> imageDelete = new();
					if (request.ImageURLs.IsNullOrEmpty())
					{
						imageDelete.AddRange(oldImgs);
					}
					else
					{
						var imgsDelete = oldImgs.Where(old => !request.ImageURLs.Contains(old.ImageURL));
						imageDelete.AddRange(imgsDelete);
					}
					_fileStorage.Delete(imageDelete.Select(e => e.ImageURL));
					await _imageRepository.DeleteAsync(imageDelete);

					if (images.Count > 0)
					{
						List<string> fileNames = new();
						var imgs = images.Select(file =>
						{
							var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
							fileNames.Add(name);
							var image = new Image()
							{
								ProductId = product.Id,
								ImageURL = Path.Combine(path, name)
							};
							return image;
						});
						await _imageRepository.AddAsync(imgs);
						await _fileStorage.SaveAsync(path, images, fileNames);
					}
					await _productRepository.UpdateAsync(product);
					return _mapper.Map<ProductDTO>(product);
				}
				catch (Exception ex)
				{
					throw new Exception(ex.InnerException?.Message ?? ex.Message);
				}
			}
			else throw new ArgumentException($"Id {id} " + ErrorMessage.NOT_FOUND);
		}

		public Task<ProductDTO> UpdateProductAsync(int id, ProductRequest request, IFormFileCollection images)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateProductEnableAsync(int id, UpdateEnableRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
