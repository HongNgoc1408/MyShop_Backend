﻿using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class ProductReview : IBaseEntity
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[MaxLength(100)]
		public string Review { get; set; }

		[Range(1, 5)]
		public int Star { get; set; }

		public int ProductId { get; set; }
		public Product Product { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
	{
	}
}