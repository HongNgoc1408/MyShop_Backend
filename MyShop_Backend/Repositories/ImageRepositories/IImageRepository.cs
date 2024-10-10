using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyShop_Backend.Repositories.ImageRepositories
{
	public interface IImageRepository : ICommonRepository<Image>
	{
		Task<Image?> GetFirstImageByProductAsync(long id);
		Task<IEnumerable<Image>> GetImageByProductIdAsync(long productId);
	}
}
