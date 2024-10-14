namespace MyShop_Backend.Request
{
	public class UpdatePaymentMethodRequest
	{
		public string? Name { get; set; }
		public bool? IsActive { get; set; }
	}
}
