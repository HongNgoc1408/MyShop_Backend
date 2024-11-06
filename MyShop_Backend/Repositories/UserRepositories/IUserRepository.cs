using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyShop_Backend.Repositories.UserRepositories
{
	public interface IUserRepository : ICommonRepository<User>
	{
		Task<IEnumerable<User>> GetAllUserAsync(int page, int pageSize);
		Task<IEnumerable<User>> GetAllUserAsync(int page, int pageSize, string search);
		//Task<int> CountAsync();
		Task<int> CountAsync(string search);
	}
}
