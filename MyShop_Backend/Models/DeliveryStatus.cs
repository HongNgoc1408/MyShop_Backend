using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class DeliveryStatus
	{
		[Key]
		public string Name { get; set; }
		public ICollection<Order> Orders { get; } = new HashSet<Order>();
	}
}
