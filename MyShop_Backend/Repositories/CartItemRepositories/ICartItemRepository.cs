using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.CartItemRepositories
{
	public interface ICartItemRepository : ICommonRepository<CartItem>
	{
		Task DeleteByCartId(string userId, IEnumerable<long> productIds);
		Task<CartItem?> SingleOrDefaultAsync(Expression<Func<CartItem, bool>> expression);

	}
}
