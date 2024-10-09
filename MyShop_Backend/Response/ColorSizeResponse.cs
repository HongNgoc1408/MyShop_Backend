using MyShop_Backend.ModelView;

namespace MyShop_Backend.Response
{
	public class ColorSizeResponse
	{
		public long Id { get; set; }
		public string ColorName { get; set; }
		public string ImageUrl { get; set; }
		public IEnumerable<SizeInStock> SizeInStocks { get; set; }
	}
}
