using MyShop_Backend.DTO;
using MyShop_Backend.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Request
{
	public class ProductRequest
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public bool Enable { get; set; } = true;
		public GenderEnum Gender { get; set; }
		public int CategoryId { get; set; }
		public int BrandId { get; set; }
		public double Price { get; set; }
		public float DiscountPercent { get; set; } = 0;
		public IEnumerable<int> MaterialIds { get; set; }
		public IEnumerable<ColorSizeRequest> ColorSizes { get; set; }

		//for update
		public IEnumerable<string>? ImageUrls { get; set; }
	}
}
