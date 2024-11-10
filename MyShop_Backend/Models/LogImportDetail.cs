﻿namespace MyShop_Backend.Models
{
	public class LogImportDetail
	{
		public long Id { get; set; }
		public long LogId { get; set; }
		public LogImport Log { get; set; }
		public string ProductName { get; set; }
		public string ColorName { get; set; }
		public string SizeName { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
	
	}
}
