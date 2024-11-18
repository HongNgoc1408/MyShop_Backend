namespace MyShop_Backend.Response
{
	public class ImportDetailResponse
	{
		public long Id { get; set; }
		public long ProductId { get; set; }
		public string ProductName { get; set; }
		public long ColorId { get; set; }
		public string ColorName { get; set; }
		public long SizeId { get; set; }
		public string SizeName { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }

	}
}
