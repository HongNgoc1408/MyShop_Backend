namespace MyShop_Backend.DTO
{
	public class LogDTO
	{
		public long Id { get; set; }
		public long ImportId { get; set; }
		public string Creator { get; set; }
		public string? Note { get; set; }
		public double Total { get; set; }
		public DateTime EntryDate { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
