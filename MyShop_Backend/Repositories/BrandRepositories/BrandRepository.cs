using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyShop_Backend.Repositories.BrandRepositories
{
	public class BrandRepository : CommonRepository<Brand>, IBrandRepository
	{
		public BrandRepository(MyShopDbContext context) : base(context) { }
	}
}
