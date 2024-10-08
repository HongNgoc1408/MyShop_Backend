using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyStore.Repository.ProductRepository
{
	public interface IProductRepository : ICommonRepository<Product>
	{
		//Task<IEnumerable<Product>> GetPageProductAsync(int page, int pageSize, string search);
		//Task<IEnumerable<Product>> GetPageProductAsync(int page, int pageSize);
		//Task<int> CountAsync(string search);
		Task<Product?> GetProductByIdAsync(int id);
	}
}