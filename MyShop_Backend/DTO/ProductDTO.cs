using MyShop_Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.DTO
{
	public class ProductDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		[Range(0, double.MaxValue)]
		public double Price { get; set; }
		[Range(0, double.MaxValue)]
		public double Discount { get; set; }
		public string? Description { get; set; }
		public int QuantitySold { get; set; }
		public int Inventory { get; set; }
		public required string Status { get; set; }
		public bool Favorite { get; set; }
		public string CategoryName { get; set; }
		public string ImageURL { get; set; }
	}
}
