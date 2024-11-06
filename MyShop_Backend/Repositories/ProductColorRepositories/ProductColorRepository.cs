using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.CommonRepository.CommonRepository;

namespace MyShop_Backend.Repositories.ProductColorRepositories
{
	public class ProductColorRepository(MyShopDbContext dbcontext) : CommonRepository<ProductColor>(dbcontext), IProductColorRepository
	{
		private readonly MyShopDbContext _dbContext = dbcontext;
	
		public async Task<IEnumerable<ProductColor>> GetColorProductAsync(long ProductId)
			=> await _dbContext.ProductColors
				.Where(e => e.ProductId == ProductId)
				.ToListAsync();

		public async Task<ProductColor?> GetFirstColorByProductAsync(long id)
		 => await _dbContext.ProductColors.FirstOrDefaultAsync(e => e.ProductId == id);

	}
}
