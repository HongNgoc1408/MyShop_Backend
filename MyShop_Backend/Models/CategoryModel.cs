using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class CategoryModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Status { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime? UpdateAt { get; set; }
		public ICollection<ProductModel> Products { get;}
	}
}
