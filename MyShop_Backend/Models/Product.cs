using MyShop_Backend.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
    public class Product
	{
        public int Id { get; set; }
        public string Name { get; set; }
		[Range(1000, double.MaxValue)]
		public double Price { get; set; }
		
		[Range(0, 100)]
		public double Discount { get; set; }
		public string? Description { get; set; }
		[Range(0, int.MaxValue)]
		public int Quantity { get; set; }

		[Range(0, int.MaxValue)]
		public int Sold {  get; set; }
		public GenderEnum Gender { get; set; }
		public bool Enable { get; set; }

		public string? Status { get; set; }
        public bool Favorite { get; set; }
		public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

		public int BrandId { get; set; }
		public Brand Brand { get; set; }

		//public ICollection<ProductColor> ProductColors { get; } = new HashSet<ProductColor>();

		//public ICollection<OrderDetail> OrderDetails { get; } = new HashSet<OrderDetail>();

		public ICollection<Image> Images { get; } = new HashSet<Image>();

		//public ICollection<ProductReview> ProductReviews { get; } = new HashSet<ProductReview>();

		//public ICollection<ProductFavorite> ProductFavorites { get; } = new HashSet<ProductFavorite>();


	}
}
