namespace MyShop_Backend.Models
{
	public interface IBaseEntity
	{
		DateTime CreatedAt { get; set; }
		DateTime? UpdatedAt { get; set; }
	}
}
