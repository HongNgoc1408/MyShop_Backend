using MyShop_Backend.Request;

namespace MyShop_Backend.DTO
{
	public class PaymentMethodDTO : CreatePaymentMethodRequest
	{
		public int Id { get; set; }
	}
}
