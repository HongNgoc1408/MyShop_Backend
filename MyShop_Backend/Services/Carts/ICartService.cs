using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Carts
{
	public interface ICartService
	{
		Task<IEnumerable<CartItemResponse>> GetAllByUserId(string userId);
		Task<IEnumerable<string>> GetCountProdutctId(string userId);
		Task AddToCart(string userId, CartRequest cartRequest);
		Task<CartItemResponse> UpdateCartItem(string cartId, string userId, UpdateCartItemRequest cartRequest);
		Task DeleteCartItem(string cartId, string userId);
		Task DeleteCartAsync(string userId, IEnumerable<long> productId);
	}
}
