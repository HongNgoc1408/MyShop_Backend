using MyShop_Backend.Data;
using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Repository.CommonRepository;

namespace MyShop_Backend.Repositories.SizeRepositories
{
	public class SizeRepository(MyShopDbContext dbcontext) : CommonRepository<Size>(dbcontext), ISizeRepository
	{}
}
