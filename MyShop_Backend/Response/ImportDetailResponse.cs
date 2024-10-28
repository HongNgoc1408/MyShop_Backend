namespace MyShop_Backend.Response
{
	public class ImportDetailResponse
	{
		public long Id { get; set; }
		public int Quantity { get; set; }
		public double OriginPrice { get; set; }

		public int ProductId { get; set; }
		public string ProductName { get; set; }
	}
}
