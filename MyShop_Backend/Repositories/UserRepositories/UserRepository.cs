using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.CommonRepository.CommonRepository;
using MyShop_Backend.Services.PagedServices;

namespace MyShop_Backend.Repositories.UserRepositories
{
	public class UserRepository : CommonRepository<User>, IUserRepository
	{
		private readonly MyShopDbContext _dbContext;

		public UserRepository(MyShopDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		
		public async Task<int> CountAsync(string search)
		{
			return await _dbContext.Users
			.Where(e => e.Id.ToString().Contains(search)
					|| (e.UserName != null && e.UserName.Contains(search))
					|| (e.Email != null && e.Email.Contains(search))
					|| (e.PhoneNumber != null && e.PhoneNumber.Contains(search)))
				.CountAsync();
		}

		public async Task<IEnumerable<User>> GetAllUserAsync(int page, int pageSize)
		{
			return (IEnumerable<User>)await _dbContext.Users
			   .Paginate(page, pageSize)
			   .ToListAsync();
		}

		public async Task<IEnumerable<User>> GetAllUserAsync(int page, int pageSize, string search)
		{
			return (IEnumerable<User>)await _dbContext.Users
				.Where(e => e.Id.ToString().Contains(search)
				|| (e.UserName != null && e.UserName.Contains(search))
				|| (e.Email != null && e.Email.Contains(search))
				|| (e.PhoneNumber != null && e.PhoneNumber.Contains(search)))
				.Paginate(page, pageSize)
				.ToListAsync();
		}
	}
}
