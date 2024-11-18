using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class User : IdentityUser, IBaseEntity
	{
		[MaxLength(50)]
		public string FullName { get; set; }
		public string PhoneNumber { get; set; }
		public DeliveryAddress? DeliveryAddress { get; set; }
		public string? ImageURL { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public ICollection<Order> Orders { get; } = new HashSet<Order>();
		public ICollection<ProductFavorite> ProductFavorites { get; } = new HashSet<ProductFavorite>();
		public ICollection<ProductReview> ProductReviews { get; } = new HashSet<ProductReview>();

		public virtual ICollection<UserRole> UserRoles { get; }
	}
}
