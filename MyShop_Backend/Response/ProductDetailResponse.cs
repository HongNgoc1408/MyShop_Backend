namespace MyShop_Backend.Request
{
	public class ProductDetailResponse
	{
		public string Name { get; set; }
		public double Price { get; set; }
		public double Discount { get; set; }
		public string? Description { get; set; }
		public int QuantitySold { get; set; }
		public int Inventory { get; set; }
		public string? Status { get; set; }
		public bool Favorite { get; set; }
		public int CategoryId { get; set; }
		public int BrandId { get; set; }
		public IEnumerable<string> ImageURLs { get; set; }
	}
}
