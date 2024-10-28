using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;

namespace MyShop_Backend.Repositories.ImportRepositories
{
	public class ImportRepository(MyShopDbContext dbcontext) : CommonRepository<Import>(dbcontext), IImportRepository
	{
		private readonly MyShopDbContext _dbcontext = dbcontext;
	}
}
