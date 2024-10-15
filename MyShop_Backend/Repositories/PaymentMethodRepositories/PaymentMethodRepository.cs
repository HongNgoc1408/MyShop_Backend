using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;

namespace MyShop_Backend.Repositories.PaymentMethodRepositories
{
	public class PaymentMethodRepository(MyShopDbContext dbcontext) : CommonRepository<PaymentMethod>(dbcontext), IPaymentMethodRepository
	{
		private readonly MyShopDbContext _dbcontext = dbcontext;
		public override async Task<IEnumerable<PaymentMethod>> GetAllAsync()
			=> await _dbcontext.PaymentMethods.OrderBy(p => p.Id).ToListAsync();

	}
}
