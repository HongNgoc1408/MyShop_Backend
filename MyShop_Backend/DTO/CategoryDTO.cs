using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.DTO
{
	public class CategoryDTO
	{
		[Key]
		public int Id { get; set; }
		public required string Name { get; set; }
		public string Status { get; set; }
	}
}
