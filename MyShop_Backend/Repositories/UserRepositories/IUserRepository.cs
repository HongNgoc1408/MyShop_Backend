using MyShop_Backend.Models;

namespace MyShop_Backend.Repositories.UserRepositories
{
	public interface IUserRepository
	{
		Task<IEnumerable<Users>> GetAllUserAsync(int page, int pageSize);
		Task<IEnumerable<Users>> GetAllUserAsync(int page, int pageSize, string search);
		//Task<int> CountAsync();
		Task<int> CountAsync(string search);
	}
}
