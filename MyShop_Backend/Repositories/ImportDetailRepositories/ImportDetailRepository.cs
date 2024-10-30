using Microsoft.EntityFrameworkCore;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using System.Linq.Expressions;

namespace MyShop_Backend.Repositories.ImportDetailRepositories
{
	public class ImportDetailRepository(MyShopDbContext dbcontext) : CommonRepository<ImportDetail>(dbcontext), IImportDetailRepository
	{
		private readonly MyShopDbContext _dbcontext = dbcontext;

		public override async Task<IEnumerable<ImportDetail>> GetAsync(Expression<Func<ImportDetail, bool>> expression)
		{
			return await _dbcontext.ImportDetails
				.Where(expression)
				.Include(e => e.Product)
				//.Include(e => e.ProductColors)
				//.Include(e => e.ProductSize)
				.ToListAsync();
		}
	}
}
