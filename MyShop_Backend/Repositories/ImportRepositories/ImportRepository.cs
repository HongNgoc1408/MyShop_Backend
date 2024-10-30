using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Services.PagedServices;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ImportRepositories
{
	public class ImportRepository(MyShopDbContext dbcontext) : CommonRepository<Import>(dbcontext), IImportRepository
	{
		private readonly MyShopDbContext _dbcontext = dbcontext;

		public override async Task<IEnumerable<Import>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<Import, bool>>? expression, Expression<Func<Import, TKey>> orderByDesc)
		=> expression == null
			? await _dbcontext.Imports
				.Paginate(page, pageSize)
				.OrderByDescending(orderByDesc)
				.Include(e => e.User)
				.ToArrayAsync()
			: await _dbcontext.Imports
				.Where(expression)
				.Paginate(page, pageSize)
				.OrderByDescending(orderByDesc)
				.Include(e => e.User)
				.ToArrayAsync();
	}
}
