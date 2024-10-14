using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repository.CommonRepository;
using System.Linq;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.CartItemRepositories
{
	public class CartItemRepository : CommonRepository<CartItem>, ICartItemRepository
	{
		private readonly MyShopDbContext _dbContext;

		public CartItemRepository(MyShopDbContext dbContext) : base(dbContext)
		{
            _dbContext = dbContext;
		}

		public async Task DeleteByCartId(string userId, IEnumerable<long> productIds)
		{
			var cartItemDelete = await _dbContext.CartItems
				.Where(e => e.UserId == userId && productIds.Contains(e.ProductId)).ToListAsync();

			_dbContext.CartItems.RemoveRange(cartItemDelete);
			await _dbContext.SaveChangesAsync();
		}
		public override async Task<IEnumerable<CartItem>> GetAsync(Expression<Func<CartItem, bool>> expression)
        {
            return await _dbContext.CartItems
                .Where(expression)
                .Include(e => e.Product)
                    .ThenInclude(e => e.ProductColors)
                    .ThenInclude(e => e.ProductSizes)
                    .ThenInclude(e => e.Size)
                .AsSingleQuery().ToListAsync();
        }

        public async Task<CartItem?> SingleOrDefaultAsync(Expression<Func<CartItem, bool>> expression)
        {
            return await _dbContext.CartItems
                .Include(e => e.Product)
                    .ThenInclude(e => e.ProductColors)
                    .ThenInclude(e => e.ProductSizes)
                    .ThenInclude(e => e.Size)
                .AsSingleQuery()
                .SingleOrDefaultAsync(expression);
        }
	}
}
