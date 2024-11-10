using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;
using MyShop_Backend.Request;

namespace MyShop_Backend.Repositories.ImportRepositories
{
	public interface IImportRepository : ICommonRepository<Import>
	{
		Task<IEnumerable<StatisticDTO>> GetTotalSpendingByYear(int year, int? month);
		Task<IEnumerable<StatisticDateDTO>> GetTotalSpending(DateTime dateFrom, DateTime dateTo);

	}
}
