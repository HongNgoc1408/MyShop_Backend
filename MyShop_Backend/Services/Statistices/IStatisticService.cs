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
		Task<StatisticResponse> GetTotalSpendingByYear(int year, int? month);
		Task<StatisticDateResponse> GetTotalSpending(DateTime dateFrom, DateTime dateTo);
		Task<StatisticResponse> GetTotalSoldByYear(int year, int? month);
		Task<StatisticDateResponse> GetTotalSold(DateTime dateFrom, DateTime dateTo);
		Task<RevenueResponse> GetRevenueByYear(int year, int? month);
		Task<RevenueDateResponse> GetRevenue(DateTime dateFrom, DateTime dateTo);
		Task<StatisticProductReponse> GetStatisticProductYear(int productId, int year, int? month);
		Task<StatisticProductReponse> GetStatisticProduct(int productId, DateTime from, DateTime to);
	}
}
