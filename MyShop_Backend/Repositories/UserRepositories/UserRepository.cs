using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repository.CommonRepository;
using MyShop_Backend.Services.PagedServices;

namespace MyShop_Backend.Repositories.UserRepositories
{
	public class UserRepository : CommonRepository<User>, IUserRepository
	{
		private readonly MyShopDbContext _context;

		public UserRepository(MyShopDbContext context) : base(context)
		{
			_context = context;
		}

		
		public async Task<int> CountAsync(string search)
		{
			return await _context.Users
			.Where(e => e.Id.ToString().Contains(search)
					|| (e.UserName != null && e.UserName.Contains(search))
					|| (e.Email != null && e.Email.Contains(search))
					|| (e.PhoneNumber != null && e.PhoneNumber.Contains(search)))
				.CountAsync();
		}

		public async Task<IEnumerable<User>> GetAllUserAsync(int page, int pageSize)
		{
			return (IEnumerable<User>)await _context.Users
			   .Paginate(page, pageSize)
			   .ToListAsync();
		}

		public async Task<IEnumerable<User>> GetAllUserAsync(int page, int pageSize, string search)
		{
			return (IEnumerable<User>)await _context.Users
				.Where(e => e.Id.ToString().Contains(search)
				|| (e.UserName != null && e.UserName.Contains(search))
				|| (e.Email != null && e.Email.Contains(search))
				|| (e.PhoneNumber != null && e.PhoneNumber.Contains(search)))
				.Paginate(page, pageSize)
				.ToListAsync();
		}
	}
}
