using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class User : IdentityUser, IBaseEntity
	{
		[MaxLength(50)]
		public string? FullName { get; set; }
		public DeliveryAddress? DeliveryAddress { get; set; }
		public ICollection<Order> Orders { get; } = new HashSet<Order>();
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
