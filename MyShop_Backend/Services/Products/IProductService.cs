using MyShop_Backend.DTO;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Products
{
	public interface IProductService
	{
		Task<ProductDTO> CreateProductAsync(ProductRequest request, IFormFileCollection images);
		Task<PagedResponse<ProductDTO>> GetAllProductsAsync(int page, int pageSize, string? keySearch);
		
		Task<PagedResponse<ProductDTO>> GetFeaturedProductsAsync(int page, int pageSize);
		Task<PagedResponse<ProductDTO>> GetFilterProductsAsync(ProductFiltersRequest filters);

		Task<IEnumerable<ProductDTO>> GetSearchProducts(string key);

		Task<ProductDetailsResponse> GetProductAsync(long id);
		Task<ProductDTO> UpdateProductAsync(long id, ProductRequest request, IFormFileCollection images);
		Task<bool> UpdateProductEnableAsync(long id, UpdateEnableRequest request);
		Task DeleteProductAsync(long id);
	}
}