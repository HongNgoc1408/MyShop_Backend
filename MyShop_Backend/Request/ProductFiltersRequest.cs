using MyShop_Backend.Enumerations;

namespace MyShop_Backend.Request
{
	public class ProductFiltersRequest : PageRequest
	{
		public SortEnum Sorter { get; set; } = 0;
		public IEnumerable<int>? CategoryIds { get; set; }
		public IEnumerable<int>? BrandIds { get; set; }
		public int? Rating { get; set; }
		public double? MinPrice { get; set; }
		public double? MaxPrice { get; set; }
		public bool? Discount { get; set; }
		public bool? FlashSale { get; set; }
	}
}
