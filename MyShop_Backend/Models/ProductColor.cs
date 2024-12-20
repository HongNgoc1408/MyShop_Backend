﻿namespace MyShop_Backend.Models
{
	public class ProductColor : IBaseEntity
	{
		public long Id { get; set; }
		public string ColorName { get; set; }

		public long ProductId { get; set; }
		public Product Product { get; set; }

		public string ImageUrl { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public ICollection<ProductSize> ProductSizes { get; } = new HashSet<ProductSize>();
	}
}
