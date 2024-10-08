using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.BrandRepositories;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyShop_Backend.Repositories.ImageRepositories
{
	public class ImageRepository : CommonRepository<Image>, IImageRepository
	{
		private readonly MyShopDbContext _dbContext;

		public ImageRepository(MyShopDbContext dbContext) : base(dbContext) {
			_dbContext = dbContext;
		}

		public async Task<Image?> GetFirstImageByProductAsync(int id)
		{
			return await _dbContext.Images.FirstOrDefaultAsync(e => e.ProductId == id);
		}

		public async Task<IEnumerable<Image>> GetImageProductAsync(int ProductId)
		{
			return await _dbContext.Images.Where(e => e.ProductId == ProductId).ToListAsync();
		}
	}
}
