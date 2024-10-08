namespace MyShop_Backend.DTO
{
	public class UserDTO
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public IList<string> Roles { get; set; }
		
	}
}
