using MyShop_Backend.Enumerations;

namespace MyShop_Backend.Request
{
	public class UpdateStatusOrderRequest
	{
		public DeliveryStatusEnum? OrderStatus { get; set; }
	}
}
