namespace MyShop_Backend.DTO
{
	public class LogDTO
	{
		public long Id { get; set; }
		public long ReceiptId { get; set; }
		public string Creator { get; set; }
		public string? Note { get; set; }
		public double Total { get; set; }
		public DateTime EntryDate { get; set; }
		public DateTime CreateAt { get; set; }
	}
}
