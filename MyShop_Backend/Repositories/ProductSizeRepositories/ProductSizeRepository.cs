using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.CommonRepository.CommonRepository;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis;

namespace MyShop_Backend.Repositories.ProductSizeRepositories
{
	public class ProductSizeRepository(MyShopDbContext dbcontext) : CommonRepository<ProductSize>(dbcontext), IProductSizeRepository
	{
		private readonly MyShopDbContext _dbContext = dbcontext;

		public async Task<IEnumerable<ProductSize>> GetSizeProductAsync(long ColorId)
		{
			return await _dbContext.ProductSizes
				.Where(e => e.ProductColorId == ColorId)
				.Include(e => e.Size)
				.ToListAsync();
		}

		public async Task<ProductSize> SingleAsyncInclude(Expression<Func<ProductSize, bool>> expression)
		{
			return await _dbContext.ProductSizes
				.Include(e => e.ProductColor)
					.ThenInclude(x => x.Product)

				.Include(e => e.Size)
				.SingleAsync(expression);
		}
	}
}
