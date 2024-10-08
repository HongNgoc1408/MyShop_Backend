using MyShop_Backend.DTO;

namespace MyShop_Backend.Services.CategoryService
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
		Task<CategoryDTO> GetByIdCategoryAsync(int id);
		Task<CategoryDTO> AddCategoryAsync(string name);
		Task<CategoryDTO> UpdateCategoryAsync(int id, string name);
		Task DeleteCategoryAsync(int id);
	}
}
