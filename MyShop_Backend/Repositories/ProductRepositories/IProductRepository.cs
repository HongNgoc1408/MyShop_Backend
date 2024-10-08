using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ProductRepositories
{
	public interface IProductRepository : ICommonRepository<Product>
	{
		Task<Product?> SingleOrDefaultAsyncInclude(Expression<Func<Product, bool>> expression);
	}
}
