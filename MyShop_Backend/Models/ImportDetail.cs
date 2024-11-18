using Microsoft.EntityFrameworkCore;

namespace MyShop_Backend.Models
{
	public class ImportDetail
	{
		public long Id { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public long ColorId { get; set; }
		public string ColorName { get; set; }
		public long SizeId { get; set; }
		public string SizeName { get; set; }
		public long ImportId { get; set; }
		public Import Import { get; set; }
		public long? ProductId { get; set; }

		[DeleteBehavior(DeleteBehavior.SetNull)]
		public Product Product { get; set; }

		public string ProductName { get; set; }

		//public ICollection<ProductColor> ProductColors { get; } = new HashSet<ProductColor>();
	}
}
