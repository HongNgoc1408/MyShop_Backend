using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyShop_Backend.Repositories.CategoryRepositories
{
	public class CategoryRepository : CommonRepository<CategoryModel>, ICategoryRepository
	{
		public CategoryRepository(MyShopDbContext context) : base(context)
		{
		}
	}
}
