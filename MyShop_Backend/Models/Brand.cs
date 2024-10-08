﻿namespace MyShop_Backend.Models
{
	public class Brand
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? ImageURL { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime? UpdateAt { get; set; }
		public ICollection<Product> Products { get; }
	}
}
