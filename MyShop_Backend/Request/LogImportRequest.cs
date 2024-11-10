namespace MyShop_Backend.Request
{
	public class LogImportRequest
	{
		public string? UserId { get; set; }
		public double Total { get; set; }
		public string? Note { get; set; }
		public DateTime EntryDate { get; set; }
		public long ImportId { get; set; }
		public IEnumerable<LogImportProduct> LogImportProducts { get; set; }
	}
	public class LogImportProduct
	{
		public string ProductName { get; set; }
		public string ColorName { get; set; }
		public string SizeName { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
	}
}
