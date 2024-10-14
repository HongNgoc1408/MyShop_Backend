using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.DeliveryAddressRepositories;
using MyShop_Backend.Repositories.UserRepositories;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.UserServices
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly IUserRepository _userRepository;
		private readonly IDeliveryAddressRepository _deliveryAddressRepository;
		private readonly IMapper _mapper;

		public UserService(UserManager<User> userManager, IUserRepository userRepository, IDeliveryAddressRepository deliveryAdressRepository, IMapper mapper) {
			_userManager = userManager;
			_userRepository = userRepository;
			_deliveryAddressRepository = deliveryAdressRepository;
			_mapper = mapper;

		}
		public async Task<PagedResponse<UserResponse>> GetAllUserAsync(int page, int pageSize, string? keySearch)
		{
			int totalUser;
			IList<User> users;
			if (keySearch == null)
			{
				totalUser = await _userRepository.CountAsync(keySearch);
				users = (await _userRepository.GetAllUserAsync(page, pageSize)).ToList();
			}
			else
			{
				totalUser = await _userRepository.CountAsync(keySearch);
				users = (await _userRepository.GetAllUserAsync(page, pageSize, keySearch)).ToList();
			}
			//var items = users
			//    .Select(x => new UserResponse
			//    {
			//        Id = x.Id,
			//        FullName = x.FullName,
			//        Email = x.Email,
			//        PhoneNumber = x.PhoneNumber,
			//    });
			var items = _mapper.Map<IList<UserResponse>>(users);
			for (int i = 0; i < users.Count(); i++)
			{
				var roles = await _userManager.GetRolesAsync(users[i]);
				if (roles != null)
				{
					items[i].Roles = roles;
				}
			}

			return new PagedResponse<UserResponse>
			{
				TotalItems = totalUser,
				Items = items,
				Page = page,
				PageSize = pageSize
			};
		}

		public async Task<AddressDTO?> GetUserAddress(string userId)
		{
			var delivery = await _deliveryAddressRepository.SingleOrDefaultAsync(e => e.User.Id == userId);
			if (delivery != null)
			{
				return _mapper.Map<AddressDTO>(delivery);
			}
			return null;
		}
		
		public async Task<AddressDTO?> UpdateUserAddress(string userId, AddressDTO address)
		{
			var delivery = await _deliveryAddressRepository.SingleOrDefaultAsync(e => e.UserId == userId);

			if (delivery == null)
			{
				delivery = new DeliveryAddress { UserId = userId, Name = address.Name };
				await _deliveryAddressRepository.AddAsync(delivery);
			}

			delivery.Province_id = address.Province_id;
			delivery.Province_name = address.Province_name;
			delivery.District_id = address.District_id;
			delivery.District_name = address.District_name;
			delivery.Ward_id = address.Ward_id;
			delivery.Ward_name = address.Ward_name;
			delivery.Detail = address.Detail;
			delivery.Name = address.Name;
			delivery.PhoneNumber = address.PhoneNumber;

			await _deliveryAddressRepository.UpdateAsync(delivery);
			return _mapper.Map<AddressDTO?>(delivery);
		}

	}
}
