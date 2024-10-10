﻿namespace MyShop_Backend.DTO
{
	public class ProductDTO
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public float Discount { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public int Sold { get; set; }
		public bool Enable { get; set; }
		public float Rating { get; set; }
		public string CategoryName { get; set; }
		public string BrandName { get; set; }
		public string ImageUrl { get; set; }

		
	}
}
