using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.CommonRepository.CommonRepository;

namespace MyShop_Backend.Repositories.ProductColorRepositories
{
	public class ProductColorRepository(MyShopDbContext dbcontext) : CommonRepository<ProductColor>(dbcontext), IProductColorRepository
	{
		private readonly MyShopDbContext _dbContext = dbcontext;
	}
}
