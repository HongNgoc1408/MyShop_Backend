using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
    public class Product 
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
		[Range(0, double.MaxValue)]
		public double Discount { get; set; }
		public string? Description { get; set; }
        public int QuantitySold { get; set; }
        public int Inventory {  get; set; }
		public required string Status { get; set; }
        public bool Favorite { get; set; }
		public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int CategoryId { get; set; }
        public required Category Category { get; set; }
        public ICollection<Image> Images { get; set; } = new HashSet<Image>();
    }
}
