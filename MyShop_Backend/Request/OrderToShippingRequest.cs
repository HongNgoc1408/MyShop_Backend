using MyShop_Backend.Enumerations;

namespace MyShop_Backend.Request
{
	public class OrderToShippingRequest
	{
		public int Weight { get; set; }
		public int Length { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public GHNRequiredNoteEnum RequiredNote { get; set; }
	}
}
