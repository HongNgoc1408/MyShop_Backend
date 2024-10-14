using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repository.CommonRepository;

namespace MyShop_Backend.Repositories.DeliveryAddressRepositories
{
	public class DeliveryAddressRepository(MyShopDbContext dbcontext)
		: CommonRepository<DeliveryAddress>(dbcontext), IDeliveryAddressRepository
	{
	}
}
