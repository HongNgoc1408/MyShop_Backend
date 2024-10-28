namespace MyShop_Backend.Models
{
	public class ImportDetail
	{
		public long Id { get; set; }
		public long ColorId { get; set; }
		//public ProductColor ProductColor { get; set; }
		public long SizeId { get; set; }
		//public ProductSize ProductSize { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public long ImportId { get; set; }
		public Import Import { get; set; }
		public long ProductId { get; set; }
		public Product Product { get; set; }
	}
}
