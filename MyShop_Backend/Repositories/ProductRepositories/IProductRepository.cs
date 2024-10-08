using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ProductRepositories
{
	public interface IProductRepository : ICommonRepository<Product>
	{
		//Task<IEnumerable<Product>> GetPageProductAsync(int page, int pageSize, string search);
		//Task<IEnumerable<Product>> GetPageProductAsync(int page, int pageSize);
		//Task<int> CountAsync(string search);
		//Task<Product?> GetProductByIdAsync(int id);
		Task<Product?> SingleOrDefaultAsyncInclude(Expression<Func<Product, bool>> expression);
	}
}
