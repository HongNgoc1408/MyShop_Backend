namespace MyShop_Backend.Request
{
	public class PageRequest
	{
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public string? Key { get; set; }
	}
}
