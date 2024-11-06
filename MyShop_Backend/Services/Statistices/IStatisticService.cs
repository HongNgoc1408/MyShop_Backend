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
		Task<StatisticResponse> GetTotalSoldByYear(int year, int? month);
		Task<StatisticDateResponse> GetTotalSpending(DateTime dateFrom, DateTime dateTo);
		Task<StatisticDateResponse> GetTotalSold(DateTime dateFrom, DateTime dateTo);
		Task<RevenueResponse> GetRevenue(int year, int? month);
	}
}
