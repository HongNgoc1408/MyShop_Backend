﻿namespace MyShop_Backend.ModelView
{
	public class SizeInStock
	{
		public int SizeId { get; set; }
		public string? SizeName { get; set; }
		public int InStock { get; set; } = 0;
	}
}
