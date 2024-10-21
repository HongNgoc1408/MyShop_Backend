using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Services.PagedServices;
using System.Collections;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ProductFavoriteRepositories
{
	public class ProductFavoriteRepository(MyShopDbContext dbContext) : CommonRepository<ProductFavorite>(dbContext), IProductFavoriteRepository
	{
		private readonly MyShopDbContext _dbcontext = dbContext;
		public override async Task<IEnumerable<ProductFavorite>> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<ProductFavorite, bool>>? expression, Expression<Func<ProductFavorite, TKey>> orderBy)
		=> expression == null
			? await _dbcontext.ProductFavorites
				.Paginate(page, pageSize)
				.Include(e => e.Product)
					.ThenInclude(e => e.Images)
				.OrderBy(orderBy)
		.ToArrayAsync()
			: await _dbcontext.ProductFavorites
				.Where(expression)
				.Paginate(page, pageSize)
				.Include(e => e.Product)
					.ThenInclude(e => e.Images)
				.OrderBy(orderBy)
				.ToArrayAsync();
	}
}
