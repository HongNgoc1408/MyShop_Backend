namespace MyShop_Backend.Models
{
	public class Log
	{
		public long Id { get; set; }
		public string? Note { get; set; }
		public double Total { get; set; }
		public DateTime EntryDate { get; set; }
		public string UserId { get; set; }
		public User User { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public long ImportId { get; set; }

		public ICollection<LogDetail> LogDetails { get; } = new HashSet<LogDetail>();
	}
}
