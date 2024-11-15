using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyShop_Backend.Repositories.ImportRepositories
{
	public interface IImportRepository : ICommonRepository<Import>
	{
		Task<IEnumerable<StatisticDTO>> GetTotalSpendingByYear(int year, int? month);
		Task<IEnumerable<StatisticDateDTO>> GetTotalSpending(DateTime dateFrom, DateTime dateTo);

		Task<IEnumerable<StatisticDTO>> GetTotalProductSpendingByYear(long productId, int year, int? month);
		Task<IEnumerable<StatisticDateDTO>> GetTotalProductSpending(long productId, DateTime dateFrom, DateTime dateTo);
	}
}
