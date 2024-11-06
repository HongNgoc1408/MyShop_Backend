using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ProductSizeRepositories
{
	public interface IProductSizeRepository: ICommonRepository<ProductSize>
	{
		Task<ProductSize> SingleAsyncInclude(Expression<Func<ProductSize, bool>> expression);
		Task<IEnumerable<ProductSize>> GetSizeProductAsync(long ColorId);
	}
}
