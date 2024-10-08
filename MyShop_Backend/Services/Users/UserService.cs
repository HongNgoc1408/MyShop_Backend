﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyShop_Backend.Mappers;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.UserRepositories;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.UserServices
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UserService(UserManager<User> userManager, IUserRepository userRepository, IMapper mapper) {
			_userManager = userManager;
			_userRepository = userRepository;
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
	}
}