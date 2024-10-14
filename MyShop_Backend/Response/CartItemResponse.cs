using MyShop_Backend.ModelView;

namespace MyShop_Backend.Response
{
    public class CartItemResponse
    {
		public string Id { get; set; }

		public long ProductId { get; set; }
		public string ProductName { get; set; }

		public int Quantity { get; set; }

		public double OriginPrice { get; set; }
		public float Discount { get; set; }
		public double Price => OriginPrice - OriginPrice * (Discount / 100.0);

		public long SizeId { get; set; }
		public string? SizeName { get; set; }

		public IEnumerable<SizeInStock> SizeInStocks { get; set; }

		public long ColorId { get; set; }
		public string? ColorName { get; set; }

		public int InStock { get; set; }

		public string? ImageUrl { get; set; }
	}
}
