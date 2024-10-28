using MyShop_Backend.Models;

namespace MyShop_Backend.Request
{
	public class ImportProduct
	{
		public long ProductId { get; set; }
		public long ColorId { get; set; }
		public long SizeId { get; set; }

		public int Quantity { get; set; }
		public double Price { get; set; }
	}
	public class ImportRequest
	{

		public double Total { get; set; }
		public string? Note { get; set; }
		public IEnumerable<ImportProduct> ImportProducts { get; set; }
	}
}
