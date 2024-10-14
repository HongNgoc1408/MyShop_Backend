using MyShop_Backend.DTO;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.UserServices
{
	public interface IUserService
	{
		Task<PagedResponse<UserResponse>> GetAllUserAsync(int page, int pageSize, string? keySearch);
		Task<AddressDTO?> GetUserAddress(string userId);
		Task<AddressDTO?> UpdateUserAddress(string userId, AddressDTO address);
	}
}
