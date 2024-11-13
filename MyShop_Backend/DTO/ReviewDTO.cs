namespace MyShop_Backend.DTO
{
	public class ReviewDTO
	{
		public string Id { get; set; }
		public string? Description { get; set; }
		public int Star { get; set; }
		public string Username { get; set; }
		public string SizeName { get; set; }
		public string ColorName { get; set; }
		public string? ImageURL { get; set; }
		public List<string>? ImagesUrls { get; set; } = new List<string>();
		public DateTime CreatedAt { get; set; }
	}
}
