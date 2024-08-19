using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class Image
	{
		[Key]
		public int Id { get; set; }
		public required string ImageURL { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime? UpdateAt { get; set; }
		public Product Products { get; set; }
	}
}
