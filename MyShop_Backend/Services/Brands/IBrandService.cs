using MyShop_Backend.DTO;

namespace MyShop_Backend.Services.BrandServices
{
	public interface IBrandService
	{
		Task<IEnumerable<BrandDTO>> GetAllBrandAsync();
		Task<BrandDTO> GetByIdBrandAsync(int id);
		Task<BrandDTO> AddBrandAsync(string name, IFormFile image);
		Task<BrandDTO> UpdateBrandAsync(int id, string name, IFormFile? image);
		Task DeleteBrandAsync(int id);
	}
}
