using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.DTO;
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

		public async Task<IEnumerable<StatisticDateDTO>> GetTotalProductSpending(long productId, DateTime dateFrom, DateTime dateTo)
		{
			return await _dbcontext.Imports
				.Where(e => e.EntryDate >= dateFrom && e.EntryDate <= dateTo.AddDays(1))
				.SelectMany(r => r.ImportDetails)
				.Where(p => p.ProductId == productId)
				.GroupBy(i => i.Import.EntryDate.Date)
				.Select(g => new StatisticDateDTO
				{
					Time = g.Key,
					Statistic = g.Sum(s => s.Quantity * s.Price)
				})
				.ToArrayAsync();
		}

		public async Task<IEnumerable<StatisticDTO>> GetTotalProductSpendingByYear(long productId, int year, int? month) => month == null
			  ? await _dbcontext.Imports
				.Where(r => r.EntryDate.Year == year)
				.SelectMany(r => r.ImportDetails)
				.Where(p => p.ProductId == productId)
				.GroupBy(i => i.Import.EntryDate.Month)
				.Select(g => new StatisticDTO
				{
					Time = g.Key,
					Statistic = g.Sum(rd => rd.Quantity * rd.Price)
				})
				.ToArrayAsync()
			: await _dbcontext.Imports
				.Where(r => r.EntryDate.Year == year && r.EntryDate.Month == month)
				.SelectMany(r => r.ImportDetails)
				.Where(p => p.ProductId == productId)
				.GroupBy(i => i.Import.EntryDate.Day)
				.Select(g => new StatisticDTO
				{
					Time = g.Key,
					Statistic = g.Sum(rd => rd.Quantity * rd.Price)
				})
				.ToArrayAsync();

		public async Task<IEnumerable<StatisticDateDTO>> GetTotalSpending(DateTime dateFrom, DateTime dateTo)
		{
			return await _dbcontext.Imports
				.Where(e => e.EntryDate >= dateFrom && e.EntryDate <= dateTo.AddDays(1))
				.GroupBy(e => new { e.EntryDate.Date })
				.Select(g => new StatisticDateDTO
				{
					Time = g.Key.Date,
					Statistic = g.Sum(x => x.Total)
				}).ToListAsync();
		}

		public async Task<IEnumerable<StatisticDTO>> GetTotalSpendingByYear(int year, int? month)
		=> month == null
			? await _dbcontext.Imports
				.Where(e => e.EntryDate.Year == year)
				.GroupBy(e => new { e.EntryDate.Month, e.EntryDate.Year })
				.Select(g => new StatisticDTO
				{
					Time = g.Key.Month,
					Statistic = g.Sum(x => x.Total)
				}).ToArrayAsync()
			: await _dbcontext.Imports
				.Where(e => e.EntryDate.Year == year && e.EntryDate.Month == month)
				.GroupBy(e => new { e.EntryDate.Day, e.EntryDate.Month })
				.Select(g => new StatisticDTO
				{
					Time = g.Key.Day,
					Statistic = g.Sum(x => x.Total)
				}).ToArrayAsync();


	}
}
