using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace MyShop_Backend.Models
{
	[PrimaryKey(nameof(UserId), nameof(ProductId))]
	public class ProductFavorite
	{
		public string UserId { get; set; }
		public User User { get; set; }
		public long ProductId { get; set; }
		public Product Product { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
