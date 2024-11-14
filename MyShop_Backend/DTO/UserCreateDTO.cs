namespace MyShop_Backend.DTO
{
	public class UserCreateDTO
	{
		public string? Email { get; set; }
		public string Password { get; set; } 
		public string FullName { get; set; }
		public string? PhoneNumber { get; set; }
		public string? ImageURL { get; set; }
		public IList<string> Roles { get; set; } = new List<string>();
		
	}
}
