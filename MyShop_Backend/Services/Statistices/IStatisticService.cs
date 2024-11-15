using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Statistices
{
	public interface IStatisticService
	{
		Task<int> GetCountUser();
		Task<int> GetCountOrder();
		Task<int> GetCountOrderDone();
		Task<int> GetCountProduct();
		Task<int> GetCountImport();
		Task<RevenueResponse> GetRevenueByYear(int year, int? month);
		Task<RevenueDateResponse> GetRevenue(DateTime dateFrom, DateTime dateTo);
		Task<RevenueResponse> GetProductRevenueByYear(long productId, int year, int? month);
		Task<RevenueDateResponse> GetProductRevenue(long productId, DateTime dateFrom, DateTime dateTo);
	}
}
