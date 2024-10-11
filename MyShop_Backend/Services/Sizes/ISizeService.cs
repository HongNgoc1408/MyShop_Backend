using MyShop_Backend.DTO;

namespace MyShop_Backend.Services.Sizes
{
	public interface ISizeService
	{
		Task<IEnumerable<SizeDTO>> GetSizesAsync();
		Task<SizeDTO> GetByIdSizeAsync(long id);
		Task<SizeDTO> AddSizeAsync(string name);
		Task<SizeDTO> UpdateSizeAsync(long id, string name);
		Task DeleteSizeAsync(long id);
	}
}
