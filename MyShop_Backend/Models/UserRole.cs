using Microsoft.AspNetCore.Identity;

namespace MyShop_Backend.Models
{
	public class UserRole : IdentityUserRole<string>
	{
		public virtual User User { get; set; }
		public virtual Role Role { get; set; }
	}
}
