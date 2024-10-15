using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
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
	}
}
