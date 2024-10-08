using MyShop_Backend.Models;

namespace MyShop_Backend.Repositories.UserRepositories
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetAllUserAsync(int page, int pageSize);
		Task<IEnumerable<User>> GetAllUserAsync(int page, int pageSize, string search);
		Task<int> CountAsync(string search);
	}
}
