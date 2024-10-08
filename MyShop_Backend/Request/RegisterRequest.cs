namespace MyShop_Backend.Request
{
	public class RegisterRequest
	{
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string PhoneNumber { get; set; }
		public string Token { get; set; }
	}
}
