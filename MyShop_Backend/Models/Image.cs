using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class Image
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
