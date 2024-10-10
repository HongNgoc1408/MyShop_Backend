using MyShop_Backend.Response;

namespace MyShop_Backend.Request
{
	public class ProductDetailsResponse
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public float Discount { get; set; } = 0;
		public double Price { get; set; }
		public int Quantity { get; set; }
		public int Sold { get; set; }
		public bool Enable { get; set; } = true;
		public float Rating { get; set; }
		public int CategoryId { get; set; }
		public int BrandId { get; set; }

		//public string ImageUrl { get; set; }
		public IEnumerable<string> ImageUrls { get; set; }
		public IEnumerable<ColorSizeResponse> ColorSizes { get; set; }
	}
}
