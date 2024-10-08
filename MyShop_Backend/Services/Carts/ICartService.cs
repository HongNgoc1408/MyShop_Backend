using Org.BouncyCastle.Asn1.Crmf;

namespace MyShop_Backend.Services.Carts
{
	public interface ICartService
	{
		Task<IEnumerable<CartItemsResponse>> GetAllByUserId(string userId);
		Task<int> CountCart(string userId);
		Task AddToCart(string userId, CartRequest cartRequest);
		Task<CartItemsResponse> UpdateCartItem(string cartId, string userId, UpdateCartItemRequest cartRequest);
	}
}
