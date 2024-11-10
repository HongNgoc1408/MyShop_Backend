using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;

namespace MyShop_Backend.Repositories.LogDetailRepositories
{
	public class LogImportDetailRepository(MyShopDbContext dbContext) : CommonRepository<LogImportDetail>(dbContext), ILogImportDetailRepository
	{
		private readonly MyShopDbContext _dbContext = dbContext;
	}
}
