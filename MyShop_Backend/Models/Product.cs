using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class Product : IBaseEntity
	{
		public long Id { get; set; }
		[MaxLength(150)]
		public string Name { get; set; }
		[MaxLength(500)]
		public string? Description { get; set; }
		  
		[Range(0, 100)]
		public float Discount { get; set; }

		[Range(1000,double.MaxValue)]
		public double Price { get; set; }

		[Range(0, int.MaxValue)]
		public int Sold { get; set; }
		public bool Enable { get; set; }
		[Range(0, 5)]
		public float Rating { get; set; }
		public int CategoryId { get; set; }
		public Category Caterory { get; set; }
		public int BrandId { get; set; }
		public Brand Brand { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public ICollection<Image> Images { get; set; } = new HashSet<Image>();
		public ICollection<ProductColor> ProductColors { get; } = new HashSet<ProductColor>();
		public ICollection<ProductFavorite> ProductFavorites { get; } = new HashSet<ProductFavorite>();
	}
}