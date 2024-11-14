namespace MyShop_Backend.Response
{
	public class UserResponse
	{
		public string Id { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? ImageURL { get; set; }
		public IList<string> Roles { get; set; } = new List<string>();
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
