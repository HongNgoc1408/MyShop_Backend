﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	[PrimaryKey(nameof(ProductColorId), nameof(SizeId))]
	public class ProductSize : IBaseEntity
	{
		public long ProductColorId { get; set; }
		public ProductColor ProductColor { get; set; }

		public long SizeId { get; set; }
		public Size Size { get; set; }

		[Range(0, int.MaxValue)]
		public int InStock { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

	}
}
