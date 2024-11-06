using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Services.PagedServices;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.OrderRepositories
{
	public class OrderRepository(MyShopDbContext context) : CommonRepository<Order>(context), IOrderRepository
	{
		private readonly MyShopDbContext _dbContext = context;

		public Task<Order?> SingleOrDefaultAsyncInclude(Expression<Func<Order, bool>> expression)
		{
			return _dbContext.Orders
				.Include(e => e.OrderDetails)
				.SingleOrDefaultAsync(expression);
		}

		public override async Task<IEnumerable<Order>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<Order, bool>>? expression, Expression<Func<Order, TKey>> orderByDesc)
		{
			return expression == null
				? await _dbContext.Orders
					.OrderByDescending(orderByDesc)
					.Paginate(page, pageSize)
					.Include(e => e.OrderDetails)
					.ToArrayAsync()
				: await _dbContext.Orders
					.Where(expression)
					.OrderByDescending(orderByDesc)
					.Paginate(page, pageSize)
					.Include(e => e.OrderDetails)
					.ToArrayAsync();
		}

		public async Task<IEnumerable<StatisticDTO>> GetTotalSoldByYear(int year, int? month)
		=> month == null
			? await _dbContext.Orders
				.Where(e => e.ReceivedDate.Year == year &&
				  (e.OrderStatus == Enumerations.DeliveryStatusEnum.Received))
				.GroupBy(e => new { e.ReceivedDate.Month, e.ReceivedDate.Year })
				.Select(g => new StatisticDTO
				{
					Time = g.Key.Month,
					Statistic = g.Sum(x => x.Total)
				}).ToArrayAsync()
			: await _dbContext.Orders
				.Where(e => e.ReceivedDate.Year == year && e.ReceivedDate.Month == month &&
				  (e.OrderStatus == Enumerations.DeliveryStatusEnum.Received ))
				.GroupBy(e => new { e.ReceivedDate.Day, e.ReceivedDate.Month })
				.Select(g => new StatisticDTO
				{
					Time = g.Key.Day,
					Statistic = g.Sum(x => x.Total)
				}).ToArrayAsync();


		public async Task<IEnumerable<StatisticDateDTO>> GetTotalSold(DateTime dateFrom, DateTime dateTo)
		{
			return await _dbContext.Orders
				.Where(e => e.ReceivedDate >= dateFrom && e.ReceivedDate <= dateTo.AddDays(1) &&
					(e.OrderStatus == Enumerations.DeliveryStatusEnum.Received ))
				.GroupBy(e => new { e.ReceivedDate.Date })
				.Select(g => new StatisticDateDTO
				{
					Time = g.Key.Date,
					Statistic = g.Sum(x => x.Total)
				}).ToArrayAsync();
		}
	}
}
