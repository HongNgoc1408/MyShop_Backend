using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;

namespace MyShop_Backend.Repositories.LogDetailRepositories
{
	public class LogDetailRepository(MyShopDbContext dbContext) : CommonRepository<LogDetail>(dbContext), ILogDetailRepository
	{
		private readonly MyShopDbContext _dbContext = dbContext;
	}
}
