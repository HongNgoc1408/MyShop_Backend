using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.Models
{
	public class ProductReview : IBaseEntity
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[MaxLength(200)]
		public string? Description { get; set; }

		[Range(1, 5)]
		public int Star { get; set; }
		//public string Variant { get; set; }
		public string SizeName { get; set; }
		public string ColorName { get; set; }

		public long? ProductId { get; set; }
		public Product? Product { get; set; }

		public string? UserId { get; set; }
		public User? User { get; set; }

		[NotMapped]
		public List<string>? ImagesUrls { get; set; }

		[Column(TypeName = "nvarchar(max)")]
		public string? ImagesUrlsJson
		{
			get => JsonConvert.SerializeObject(ImagesUrls);
			set => ImagesUrls = value == null ? null : JsonConvert.DeserializeObject<List<string>>(value);
		}
		public bool Enable {  get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	} 
}
