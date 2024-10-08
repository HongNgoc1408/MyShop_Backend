using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CategoryRepositories;

namespace MyShop_Backend.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;

		public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}
		public async Task<CategoryDTO> AddCategoryAsync(string name)
		{
			var category = new Category
			{
				Name = name
			};
			await _categoryRepository.AddAsync(category);
			return _mapper.Map<CategoryDTO>(category);

		}

		public async Task DeleteCategoryAsync(int id)
		{
			await _categoryRepository.DeleteAsync(id);
		}

		public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
		{
			var categories = await _categoryRepository.GetAllAsync();
			var sortedCategories = categories.OrderBy(c => c.Name); 
			return _mapper.Map<IEnumerable<CategoryDTO>>(sortedCategories);
		}

		public async Task<CategoryDTO> GetByIdCategoryAsync(int id)
		{
			var category = await _categoryRepository.FindAsync(id);

			if (category == null)
			{
				throw new ArgumentException($"ID {id}"
					+ ErrorMessage.NOT_FOUND);
			}
			else
			{
				return _mapper.Map<CategoryDTO>(category);
			}
		}

		public async Task<CategoryDTO> UpdateCategoryAsync(int id, string name)
		{
			var category = await _categoryRepository.FindAsync(id);
			if (category == null)
			{
				throw new ArgumentException($"ID {id}"
					+ ErrorMessage.NOT_FOUND);
			}
			else
			{
				category.Name = name;
				await _categoryRepository.UpdateAsync(category);
				return _mapper.Map<CategoryDTO>(category);
			}
		}
	}
}
