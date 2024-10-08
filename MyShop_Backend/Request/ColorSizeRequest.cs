using MyShop_Backend.ModelView;

namespace MyShop_Backend.Request
{
	public class ColorSizeRequest
	{
		public string ColorName { get; set; }
		public IFormFile? Image { get; set; }
		public IEnumerable<SizeInStock> SizeInStocks { get; set; }

		//update
		public long? Id { get; set; }
		public string? ImageUrl { get; set; }
	}
}
