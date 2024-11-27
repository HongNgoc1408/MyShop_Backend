using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Services.PagedServices;
using MyShop_Backend.CommonRepository.CommonRepository;
using System.Linq.Expressions;
using MyStore.Repository.ProductRepository;

namespace MyShop_Backend.CommonRepository.ProductRepository
{
	public class ProductRepository : CommonRepository<Product>, IProductRepository
	{
		private readonly MyShopDbContext _dbContext;
		public ProductRepository(MyShopDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		//public async Task<int> CountAsync(string search)
		//{
		//    return await _dbContext.Products
		//        .Where(e => e.Name.Contains(search) || e.Id.ToString().Equals(search))
		//        .CountAsync() ;
		//}

		public async Task<IEnumerable<Product>> GetPageProductAsync(int page, int pageSize, string search)
		{
			return await _dbContext.Products
				.Where(e => e.Name.Contains(search) || e.Id.ToString().Equals(search))
				.Include(e => e.Brand)
				.Include(e => e.Caterory)
				.Include(e => e.Images)
				.OrderByDescending(e => e.CreatedAt)
				.Paginate(page, pageSize)
				.ToListAsync();
		}

		public async Task<Product?> GetProductByIdAsync(long id)
		{
			return await _dbContext.Products
				.Include(e => e.Brand)
				.Include(e => e.Caterory)
				.Include(e => e.Images)
				.Include(e => e.ProductColors)
				.SingleOrDefaultAsync(e => e.Id == id);
		}

		public async Task<IEnumerable<Product>> GetPageProductAsync(int page, int pageSize)
		{
			return await _dbContext.Products
					.Include(e => e.Caterory)
					.Include(e => e.Brand)
					.Include(e => e.Images)
					.Paginate(page, pageSize)
					.ToListAsync();
		}

		public override async Task<Product?> SingleOrDefaultAsync(Expression<Func<Product, bool>> expression)
		{
			return await _dbContext.Products
			   .Include(e => e.Images)
			   .Include(e => e.ProductColors).ThenInclude(x => x.ProductSizes)
			   .SingleOrDefaultAsync(expression);
		}

		public override async Task<IEnumerable<Product>> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<Product, bool>>? expression, Expression<Func<Product, TKey>> orderBy)
		{
			return expression == null
				? await _dbContext.Products
					.Include(e => e.Brand)
					.Include(e => e.Images)
					.Include(e => e.Caterory).OrderBy(orderBy).Paginate(page, pageSize).ToListAsync()
				: await _dbContext.Products.Where(expression)
					.Include(e => e.Brand)
					.Include(e => e.Images)
					.Include(e => e.Caterory).OrderBy(orderBy).Paginate(page, pageSize).ToListAsync();
		}

		public override async Task<IEnumerable<Product>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<Product, bool>>? expression, Expression<Func<Product, TKey>> orderByDesc)
		{
			return expression == null
				? await _dbContext.Products
					.OrderByDescending(orderByDesc)
					.Paginate(page, pageSize)
					.Include(e => e.Brand)
					.Include(e => e.Caterory)
					.Include(e => e.Images)
					.AsSingleQuery()
					
					.ToListAsync()
				: await _dbContext.Products
					.Where(expression)
					.OrderByDescending(orderByDesc)
					.Paginate(page, pageSize)
					.Include(e => e.Brand)
					.Include(e => e.Caterory)
					.Include(e => e.Images)
					.AsSingleQuery()
					.ToListAsync();
		}
	}
}