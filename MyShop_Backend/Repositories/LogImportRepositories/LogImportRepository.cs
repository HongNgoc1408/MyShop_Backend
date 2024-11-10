using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;

namespace MyShop_Backend.Repositories.LogRepositories
{
	public class LogImportRepository(MyShopDbContext dbContext) : CommonRepository<LogImport>(dbContext), ILogImportRepository
	{
		private readonly MyShopDbContext _dbContext = dbContext;
	}
}
