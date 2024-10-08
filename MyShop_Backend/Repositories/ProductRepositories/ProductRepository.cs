using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.BrandRepositories;
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
			.Include(e => e.Brand)
			.AsSplitQuery()
			.SingleOrDefaultAsync(expression);
		}
		

		public async Task<int> CountAsync(string search)
		{
			return await _dbContext.Products.Where(e => e.Name.Contains(search) || e.Id.ToString().Equals(search)).CountAsync();
		}

		public override async Task<IEnumerable<Product>> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<Product, bool>>? expression, Expression<Func<Product, TKey>> orderBy)
			=> expression == null
				? await _dbContext.Products
						.Paginate(page, pageSize)
						.Include(e => e.Images)
						.Include(e => e.Category)
						.Include(e => e.Brand).OrderBy(orderBy).ToArrayAsync()
				: await _dbContext.Products
						.Where(expression)
						.Paginate(page, pageSize)
						.Include(e => e.Images)
						.Include(e => e.Category)
						.Include(e => e.Brand).OrderBy(orderBy).ToArrayAsync();
		public override async Task<IEnumerable<Product>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<Product, bool>>? expression, Expression<Func<Product, TKey>> orderByDesc)
			=> expression == null
				? await _dbContext.Products
						.Paginate(page, pageSize)
						.Include(e => e.Images)
						.Include(e => e.Category)
						.Include(e => e.Brand)
						.OrderByDescending(orderByDesc).ToArrayAsync()
				: await _dbContext.Products
						.Where(expression)
						.Paginate(page, pageSize)
						.Include(e => e.Images)
						.Include(e => e.Category)
						.Include(e => e.Brand)
						.OrderByDescending(orderByDesc).ToArrayAsync();
	}

	

		public async Task<Product?> GetProductByIdAsync(int id)
		{
			return await _dbContext.Products
			   .Include(e => e.Brand)
			   .Include(e => e.Category)
			   .Include(e => e.Images)
			   .SingleOrDefaultAsync(e => e.Id == id);
		}
	}
}
