using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Orders
{
	public interface IOrderService
	{
		Task<string?> CreateOrder(string userId, OrderRequest request);
		Task<PagedResponse<OrderDTO>> GetAll(int page, int pageSize, string? keySearch);
		Task<PagedResponse<OrderDTO>> GetOrdersByUserId(string userId, PageRequest request);
		Task<OrderDetailsResponse> GetOrderDetail(long orderId, string userId);
		Task<OrderDetailsResponse> GetOrderDetail(long orderId);
		//Task<OrderDTO> UpdateOrder(long orderId, string userId, UpdateOrderRequest request);
		Task<OrderDTO> UpdateOrder(long orderId, UpdateStatusOrderRequest request);
		Task ReceivedOrder(long orderId, string userId, UpdateStatusOrderRequest request);
		Task CancelOrder(long orderId, string userId);
		//Task CancelOrder(long orderId);
		Task DeleteOrder(long orderId);
		//Task NextOrderStatus(long orderId);
		Task OrderToShipping(long orderId, OrderToShippingRequest request);
		//Task<PagedResponse<OrderDTO>> GetWithOrderStatus(DeliveryStatusEnum statusEnum, PageRequest request);
		//Task<PagedResponse<OrderDTO>> GetWithOrderStatusUser(string userId, DeliveryStatusEnum statusEnum, PageRequest request);
		Task Review(long orderId, string userId, IEnumerable<ReviewRequest> reviews);
		//Task SendEmail(Order order, IEnumerable<OrderDetail> orderDetail);
	}
}
