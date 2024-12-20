﻿using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class Image : IBaseEntity
	{
		public long Id { get; set; }

		public long ProductId { get; set; }
		public Product Product { get; set; }

		public string ImageUrl { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
