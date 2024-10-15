using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.OrderDetailRepositories
{
	public class OrderDetailRepository(MyShopDbContext dbcontext) : CommonRepository<OrderDetail>(dbcontext), IOrderDetailRepository
    {
        private readonly MyShopDbContext _dbContext = dbcontext;

        public override async Task<IEnumerable<OrderDetail>> GetAsync(Expression<Func<OrderDetail, bool>> expression)
        {
            return await _dbContext.OrderDetails
                .Where(expression)
                .Include(e => e.Product)
                .ToListAsync();
        }
    }
}
