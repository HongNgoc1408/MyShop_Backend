using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;
using MyShop_Backend.Services.PagedServices;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ProductRepositories
{
	public class ProductRepository(MyShopDbContext dbcontext) : CommonRepository<Product>(dbcontext), IProductRepository
	{
		private readonly MyShopDbContext _dbContext = dbcontext;

		public async Task<Product?> SingleOrDefaultAsyncInclude(Expression<Func<Product, bool>> expression)
		{
			return await _dbContext.Products
			.Include(e => e.Images)
			.Include(e => e.ProductColors)
				.ThenInclude(e => e.ProductSizes)
					.ThenInclude(e => e.Size)
			.Include(e => e.Category)
			.Include(e => e.Brands)
			.AsSplitQuery()
			.SingleOrDefaultAsync(expression);
		}
	}
}