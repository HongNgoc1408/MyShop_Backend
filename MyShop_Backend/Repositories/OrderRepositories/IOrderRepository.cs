using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.OrderRepositories
{
	public interface IOrderRepository : ICommonRepository<Order>
	{
		Task<Order?> SingleOrDefaultAsync(Expression<Func<Order, bool>> expression);
	}
}
