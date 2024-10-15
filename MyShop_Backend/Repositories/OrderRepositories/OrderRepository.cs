using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
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
				? await _dbContext.Orders.OrderByDescending(orderByDesc).Paginate(page, pageSize).Include(e => e.OrderDetails).ToArrayAsync()
				: await _dbContext.Orders.Where(expression).OrderByDescending(orderByDesc).Paginate(page, pageSize)
				.Include(e => e.OrderDetails).ToArrayAsync();
		}
	}
}
