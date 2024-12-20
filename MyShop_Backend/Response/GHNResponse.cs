﻿namespace MyShop_Backend.Response
{
	public class GHNData
	{
		public string Order_code { get; set; }
		public DateTime Expected_delivery_time { get; set; }
	}
	public class GHNResponse
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public GHNData? Data { get; set; }
		public string Code_message { get; set; }
	}
}
