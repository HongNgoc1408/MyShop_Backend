using MyShop_Backend.DTO;

namespace MyShop_Backend.Services.Sizes
{
	public interface ISizeService
	{
        Task<IEnumerable<SizeDTO>> GetSizesAsync();
		Task<SizeDTO> AddSizeAsync(string name);
		Task<SizeDTO> UpdateSizeAsync(int id, string name);
		Task DeleteSizeAsync(int id);
	}
}

