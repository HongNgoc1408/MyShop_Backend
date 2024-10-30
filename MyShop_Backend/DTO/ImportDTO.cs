using MyShop_Backend.Models;

namespace MyShop_Backend.DTO
{
	public class ImportDTO
	{
		public long Id { get; set; }
		public string Creator { get; set; }
		public string? Note { get; set; }
		public double Total { get; set; }
		public DateTime EntryDate { get; set; }
		public DateTime CreateAt { get; set; }
	}
}
