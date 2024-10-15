namespace MyShop_Backend.Request
{
	public class VNPayQueryDr
	{
		public string vnp_RequestId { get; set; }
		public string vnp_Version { get; set; }
		public string vnp_Command { get; set; }
		public string vnp_TmnCode { get; set; }
		public string vnp_TxnRef { get; set; }
		public string vnp_TransactionDate { get; set; }
		public string vnp_CreateDate { get; set; }
		public string vnp_IpAddr { get; set; }
		public string vnp_OrderInfo { get; set; }
	}
}
