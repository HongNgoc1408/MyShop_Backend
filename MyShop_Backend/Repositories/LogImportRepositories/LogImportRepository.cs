using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Services.PagedServices;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.LogRepositories
{
	public class LogImportRepository(MyShopDbContext dbContext) : CommonRepository<LogImport>(dbContext), ILogImportRepository
	{
		private readonly MyShopDbContext _dbContext = dbContext;

		public override async Task<IEnumerable<LogImport>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<LogImport, bool>>? expression, Expression<Func<LogImport, TKey>> orderByDesc)
		=> expression == null
			? await _dbContext.LogImports
				.Paginate(page, pageSize)
				.OrderByDescending(orderByDesc)
				.Include(e => e.User)
				.ToArrayAsync()
			: await _dbContext.LogImports
				.Where(expression)
				.Paginate(page, pageSize)
				.OrderByDescending(orderByDesc)
				.Include(e => e.User)
				.ToArrayAsync();
	}
}
