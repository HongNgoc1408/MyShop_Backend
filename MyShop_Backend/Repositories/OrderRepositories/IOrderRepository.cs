using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.OrderRepositories
{
	public interface IOrderRepository : ICommonRepository<Order>
	{
		Task<Order?> SingleOrDefaultAsyncInclude(Expression<Func<Order, bool>> expression);
		Task<IEnumerable<StatisticDTO>> GetTotalSoldByYear(int year, int? month);
		Task<IEnumerable<StatisticDateDTO>> GetTotalSold(DateTime dateFrom, DateTime dateTo);
		Task<IEnumerable<StatisticDTO>> GetTotalProductSalesByYear(long productId, int year, int? month);
		Task<IEnumerable<StatisticDateDTO>> GetTotalProductSales(long productId, DateTime dateFrom, DateTime dateTo);
	}
}
