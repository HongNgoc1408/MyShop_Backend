using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Services.PagedServices;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.LogRepositories
{
	public class LogRepository(MyShopDbContext dbContext) : CommonRepository<Log>(dbContext), ILogRepository
	{
		private readonly MyShopDbContext _dbContext = dbContext;

		public override async Task<IEnumerable<Log>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<Log, bool>>? expression, Expression<Func<Log, TKey>> orderByDesc)
		=> expression == null
			? await _dbContext.Logs
				.Paginate(page, pageSize)
				.OrderByDescending(orderByDesc)
				.Include(e => e.User)
				.ToArrayAsync()
			: await _dbContext.Logs
				.Where(expression)
				.Paginate(page, pageSize)
				.OrderByDescending(orderByDesc)
				.Include(e => e.User)
				.ToArrayAsync();
	}
}
