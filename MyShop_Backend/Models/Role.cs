using Microsoft.AspNetCore.Identity;

namespace MyShop_Backend.Models
{
	public class Role : IdentityRole
	{
		public virtual ICollection<UserRole> UserRoles { get; }
	}
}
