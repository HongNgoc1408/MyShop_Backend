using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.CommonRepository.CommonRepository;

namespace MyShop_Backend.Repositories.ImageRepositories
{
	public class ImageRepository(MyShopDbContext dbcontext) : CommonRepository<Image>(dbcontext), IImageRepository
	{
		private readonly MyShopDbContext _dbContext = dbcontext;

		public async Task<Image?> GetFirstImageByProductAsync(long id)
		{
			return await _dbContext.Images.FirstOrDefaultAsync(e => e.ProductId == id);
		}

		public async Task<IEnumerable<Image>> GetImageByProductIdAsync(long productId)
			=> await _dbContext.Images.Where(e => e.ProductId == productId).ToListAsync();
	}
}
