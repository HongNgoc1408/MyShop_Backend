﻿using MyShop_Backend.DTO;
using MyShop_Backend.Enumerations;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.UserServices
{
	public interface IUserService
	{
		Task<UserDTO> AddUser(UserCreateDTO user);
		Task<UserDTO> UpdateUser(string userId, UpdateUserRequest request);
		Task<UserDTO> GetUser(string userId);
		Task<PagedResponse<UserResponse>> GetAllAsync(int page, int pageSize, string? key, RolesEnum role);

		Task<AddressDTO?> GetUserAddress(string userId);
		Task<AddressDTO?> UpdateUserAddress(string userId, AddressDTO address);
		Task<UserDTO> GetUserInfo(string userId);
		Task<UpdateInfoRequest> UpdateUserInfo(string userId, UpdateInfoRequest request);
		Task<UserDTO> UpdateAvatar(string userId, IFormFile image);
		Task<ImageDTO> GetAvatar(string userId);
		Task<IEnumerable<long>> GetFavorites(string userId);
		Task<PagedResponse<ProductDTO>> GetProductsFavorite(string userId, PageRequest request);
		Task AddProductFavorite(string userId, long productId);
		Task DeleteProductFavorite(string userId, long productId);

		Task LockOut(string userId, DateTimeOffset? endDate);

	}
}
