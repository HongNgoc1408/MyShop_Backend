using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class Image
	{
		public int Id { get; set; }
		public string ImageURL { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime? UpdateAt { get; set; }
		public int ProductId { get; set; }
		public ProductModel Products { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
