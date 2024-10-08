using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class Size : IBaseEntity
	{
		public int Id { get; set; }

		[MaxLength(30)]
		public string Name { get; set; }

		public ICollection<ProductSize> ProductSizes { get; } = new HashSet<ProductSize>();

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
