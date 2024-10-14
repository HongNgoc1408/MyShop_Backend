namespace MyShop_Backend.Request
{
	public class CreatePaymentMethodRequest
	{
		public string Name { get; set; }
		public bool IsActive { get; set; } = false;
	}
}
