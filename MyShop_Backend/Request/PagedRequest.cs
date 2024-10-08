namespace MyShop_Backend.Request
{
	public class PagedRequest
	{
		public int page { get; set; } = 1;
		public int pageSize { get; set; } = 10;
		public string? search { get; set; }
	}
}
