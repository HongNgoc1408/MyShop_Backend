namespace MyShop_Backend.DTO
{
	public class ProductOrderDetails
	{
		public long? ProductId { get; set; }
		public string ProductName { get; set; }

		public string SizeName { get; set; }
		public string ColorName { get; set; }

		public double OriginPrice { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public string? ImageUrl { get; set; }

	}
}
