using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyShop_Backend.Repositories.ProductColorRepositories
{
	public interface IProductColorRepository : ICommonRepository<ProductColor>
	{
		Task<ProductColor?> GetFirstColorByProductAsync(long id);
		Task<IEnumerable<ProductColor>> GetColorProductAsync(long ProductId);
	}
}
