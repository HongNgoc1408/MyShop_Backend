using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class User : IdentityUser, IBaseEntity
	{
		[MaxLength(50)]
		public string? FullName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
