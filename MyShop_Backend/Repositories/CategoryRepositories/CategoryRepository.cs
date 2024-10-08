using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repository.CommonRepository;

namespace MyShop_Backend.Repositories.CategoryRepositories
{
	public class CategoryRepository : CommonRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(MyShopDbContext context) : base(context)
		{
		}
	}
}
