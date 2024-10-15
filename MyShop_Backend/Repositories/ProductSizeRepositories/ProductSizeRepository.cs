using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.CommonRepository.CommonRepository;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ProductSizeRepositories
{
	public class ProductSizeRepository(MyShopDbContext dbcontext) : CommonRepository<ProductSize>(dbcontext), IProductSizeRepository
	{
		private readonly MyShopDbContext _dbContext = dbcontext;

		public async Task<ProductSize> SingleAsyncInclude(Expression<Func<ProductSize, bool>> expression)
		{
			return await _dbContext.ProductSizes
				.Include(e => e.ProductColor)
				.Include(e => e.Size)
				.SingleAsync(expression);
		}
	}
}
