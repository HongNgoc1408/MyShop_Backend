using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Services.PagedServices;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ProductReviewRepositories
{
	public class ProductReviewRepository(MyShopDbContext dbContext) : CommonRepository<ProductReview>(dbContext), IProductReviewRepository
	{
		private readonly MyShopDbContext _dbContext = dbContext;
		public override async Task<IEnumerable<ProductReview>> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<ProductReview, bool>>? expression, Expression<Func<ProductReview, TKey>> orderBy)
			=> expression == null
				? await _dbContext.ProductReviews
					.OrderBy(orderBy)
					.Paginate(page, pageSize)
					.Include(e => e.User).ToArrayAsync()
				: await _dbContext.ProductReviews
					.Where(expression)
					.OrderBy(orderBy)
					.Paginate(page, pageSize)
					.Include(e => e.User).ToArrayAsync();
		public override async Task<IEnumerable<ProductReview>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<ProductReview, bool>>? expression, Expression<Func<ProductReview, TKey>> orderByDesc)
			 => expression == null
					? await _dbContext.ProductReviews
						.OrderByDescending(orderByDesc)
						.Paginate(page, pageSize)
						.Include(e => e.User).ToArrayAsync()
					: await _dbContext.ProductReviews
						.Where(expression)
						.OrderByDescending(orderByDesc)
						.Paginate(page, pageSize)
						.Include(e => e.User).ToArrayAsync();
	}
}

