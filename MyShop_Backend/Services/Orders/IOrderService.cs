using MyShop_Backend.DTO;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Orders
{
	public interface IOrderService
	{
		Task<PagedResponse<OrderDTO>> GetOrdersByUserId(string userId, PageRequest request);
		Task<OrderDetailsResponse> GetOrderDetail(long orderId, string userId);
		Task<OrderDTO> UpdateOrder(long orderId, string userId, UpdateOrderRequest request);
		Task<OrderDTO> UpdateOrder(long orderId, UpdateStatusOrderRequest request);
		//Task<OrderDTO> UpdateStatusOrder(long orderId, string userId, UpdateOrderRequest request);
		Task CancelOrder(long orderId, string userId);
		Task<string?> CreateOrder(string userId, OrderRequest request);

		//Task Review(long orderId, string userId, IEnumerable<ReviewRequest> reviews);

		Task<PagedResponse<OrderDTO>> GetAll(int page, int pageSize, string? keySearch);
		Task<OrderDetailsResponse> GetOrderDetail(long orderId);
		Task DeleteOrder(long orderId);
	}
}
