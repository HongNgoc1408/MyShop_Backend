namespace MyShop_Backend.Models
{
	public class Import : IBaseEntity
	{
		public long Id { get; set; }
		public string? Note { get; set; }
		public double Total { get; set; }
		public DateTime EntryDate { get; set; } = DateTime.Now;
		public string? UserId { get; set; }
		public User? User { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public ICollection<ImportDetail> ImportDetails { get; } = new HashSet<ImportDetail>();
		
	}
}
