using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		public required string Name { get; set; }
		public string Status { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime? UpdateAt { get; set; }
		public ICollection<Product> Products { get; set; } = new HashSet<Product>();
	}
}
