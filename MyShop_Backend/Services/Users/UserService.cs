using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyShop_Backend.DTO;
using MyShop_Backend.Enumerations;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.DeliveryAddressRepositories;
using MyShop_Backend.Repositories.ProductFavoriteRepositories;
using MyShop_Backend.Repositories.TransactionRepositories;
using MyShop_Backend.Repositories.UserRepositories;
using MyShop_Backend.Request;
using MyShop_Backend.Response;
using MyShop_Backend.Storages;
using System.Linq.Expressions;

namespace MyShop_Backend.Services.UserServices
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly IUserRepository _userRepository;
		private readonly IDeliveryAddressRepository _deliveryAddressRepository;
		private readonly IProductFavoriteRepository _productFavoriteRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IFileStorage _fileStorage;
		private readonly string path = "assets/images/avatars";
		private readonly IMapper _mapper;

		public UserService(UserManager<User> userManager, IUserRepository userRepository, IDeliveryAddressRepository deliveryAdressRepository, IProductFavoriteRepository productFavoriteRepository, ITransactionRepository transactionRepository, IFileStorage fileStorage, IMapper mapper)
		{
			_userManager = userManager;
			_userRepository = userRepository;
			_deliveryAddressRepository = deliveryAdressRepository;
			_productFavoriteRepository = productFavoriteRepository;
			_transactionRepository = transactionRepository;
			_fileStorage = fileStorage;
			_mapper = mapper;
		}

		public async Task<UserDTO> AddUser(UserCreateDTO user)
		{
			using (await _transactionRepository.BeginTransactionAsync())
			{
				try
				{
					var User = new User()
					{
						Email = user.Email,
						NormalizedEmail = user.Email,
						UserName = user.Email,
						NormalizedUserName = user.Email,
						PhoneNumber = user.PhoneNumber,
						FullName = user.FullName,
						SecurityStamp = Guid.NewGuid().ToString(),
						ConcurrencyStamp = Guid.NewGuid().ToString()

					};
					var result = await _userManager.CreateAsync(User, user.Password);
					if (!result.Succeeded)
					{
						throw new Exception(ErrorMessage.INVALID);
					}
					var roleResult = await _userManager.AddToRolesAsync(User, user.Roles);
					if (!roleResult.Succeeded)
					{
						throw new Exception(ErrorMessage.INVALID);
					}
					await _transactionRepository.CommitTransactionAsync();
					return new UserDTO
					{
						Id = User.Id,
						Email = User.Email,
						PhoneNumber = User.PhoneNumber,
						FullName = User.FullName,
						Roles = user.Roles,
					};
				}
				catch (Exception ex)
				{
					await _transactionRepository.RollbackTransactionAsync();
					throw new Exception(ex.Message);
				}
			}
		}
		public async Task<UserDTO> UpdateUser(string userId, UpdateUserRequest request)
		{
			using (await _transactionRepository.BeginTransactionAsync())
			{
				try
				{
					var user = await _userManager.FindByIdAsync(userId);

					if (user == null)
					{
						throw new Exception(ErrorMessage.NOT_FOUND);
					}

					if (!string.IsNullOrEmpty(request.FullName))
					{
						user.FullName = request.FullName;
					}

					user.PhoneNumber = request.PhoneNumber;

					var updateRes = await _userManager.UpdateAsync(user);

					if (!updateRes.Succeeded)
					{
						throw new Exception(ErrorMessage.INVALID);
					}

					var currentRoles = await _userManager.GetRolesAsync(user);
					var removeRole = await _userManager.RemoveFromRolesAsync(user, currentRoles);
					if (!removeRole.Succeeded)
					{
						throw new Exception(ErrorMessage.INVALID);
					}

					var addRoles = await _userManager.AddToRolesAsync(user, request.Roles);
					if (!addRoles.Succeeded)
					{
						throw new Exception(ErrorMessage.INVALID);
					}
					await _transactionRepository.CommitTransactionAsync();
					return new UserDTO
					{
						Id = user.Id,
						Email = user.Email,
						PhoneNumber = user.PhoneNumber,
						FullName = user.FullName,
						Roles = request.Roles
					};
				}
				catch (Exception ex)
				{
					await _transactionRepository.RollbackTransactionAsync();
					throw new Exception(ex.Message);
				}
			}
		}
		public async Task<UserDTO> GetUser(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId)
				?? throw new Exception(ErrorMessage.NOT_FOUND);

			var roles = await _userManager.GetRolesAsync(user);

			return new UserDTO
			{
				Id = user.Id,
				Email = user.Email,
				FullName = user.FullName,
				Roles = roles,
				PhoneNumber = user.PhoneNumber
			};
		}
		public async Task<PagedResponse<UserResponse>> GetAllAsync(int page, int pageSize, string? key, RolesEnum role)
		{
			int totalUser;
			IEnumerable<User> users;

			if (string.IsNullOrEmpty(key))
			{
				totalUser = await _userRepository.CountAsync(e =>
					e.UserRoles.Any(e => !string.IsNullOrEmpty(e.Role.Name) && e.Role.Name.Equals(role.ToString())));
				users = await _userRepository.GetPagedOrderByDescendingAsync(page, pageSize,
					e => e.UserRoles.Any(e => !string.IsNullOrEmpty(e.Role.Name) && e.Role.Name.Equals(role.ToString())),
					e => e.CreatedAt);
			}
			else
			{
				Expression<Func<User, bool>> expression = e => (e.Id.Contains(key)
					|| (e.FullName != null && e.FullName.Contains(key))
					|| (e.Email != null && e.Email.Contains(key))
					|| e.PhoneNumber != null && e.PhoneNumber.Contains(key))
					&& e.UserRoles.Any(e => !string.IsNullOrEmpty(e.Role.Name) && e.Role.Name.Equals(role.ToString()));

				totalUser = await _userRepository.CountAsync(expression);
				users = await _userRepository.GetPagedOrderByDescendingAsync(page, pageSize, expression, e => e.CreatedAt);
			}

			//var items = _mapper.Map<IEnumerable<UserResponse>>(users).Select(e =>
			//{
			//    e.LockedOut = e.LockoutEnd > DateTime.Now;
			//    e.LockoutEnd = e.LockoutEnd > DateTime.Now ? e.LockoutEnd : null;
			//    return e;
			//});

			//var roles = await _userManager.GetUsersInRoleAsync("User");
			var items = _mapper.Map<IEnumerable<UserResponse>>(users);

			return new PagedResponse<UserResponse>
			{
				Items = items,
				TotalItems = totalUser,
				Page = page,
				PageSize = pageSize,
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
		private string MaskEmail(string email)
		{
			var emailParts = email.Split('@');
			if (emailParts.Length != 2)
			{
				throw new ArgumentException("Email không hợp lệ");
			}

			string name = emailParts[0];
			string domain = emailParts[1];

			int visibleChars = name.Length < 5 ? 2 : 5;
			string maskedName = name[..visibleChars].PadRight(name.Length, '*');

			return $"{maskedName}@{domain}";
		}
		public async Task<UserDTO> GetUserInfo(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				var res = _mapper.Map<UserDTO>(user);
				res.Email = res.Email != null ? MaskEmail(res.Email) : "";
				return res;
			}
			throw new InvalidOperationException(ErrorMessage.USER_NOT_FOUND);
		}

		public async Task<UpdateInfoRequest> UpdateUserInfo(string userId, UpdateInfoRequest request)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				user.FullName = request.FullName;
				user.PhoneNumber = request.PhoneNumber;
				await _userManager.UpdateAsync(user);

				return _mapper.Map<UpdateInfoRequest>(user);
			}
			throw new InvalidOperationException(ErrorMessage.NOT_FOUND);
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

		public async Task<IEnumerable<long>> GetFavorites(string userId)
		{
			var favorites = await _productFavoriteRepository.GetAsync(e => e.UserId == userId);
			return favorites.Select(e => e.ProductId);
		}

		public async Task<PagedResponse<ProductDTO>> GetProductsFavorite(string userId, PageRequest page)
		{
			var favorites = await _productFavoriteRepository.GetPagedAsync(page.Page, page.PageSize, e => e.UserId == userId, e => e.CreatedAt);

			var total = await _productFavoriteRepository.CountAsync(e => e.UserId == userId);

			var products = favorites.Select(e => e.Product).ToList();

			var items = _mapper.Map<IEnumerable<ProductDTO>>(products).Select(x =>
			{
				var image = products.Single(e => e.Id == x.Id).Images.FirstOrDefault();
				if (image != null)
				{
					x.ImageUrl = image.ImageUrl;
				}
				return x;
			});
			return new PagedResponse<ProductDTO>
			{
				Items = items,
				Page = page.Page,
				PageSize = page.PageSize,
				TotalItems = total,
			};
		}

		public async Task AddProductFavorite(string userId, long productId)
		{
			var favorites = new ProductFavorite
			{
				UserId = userId,
				ProductId = productId,
			};
			await _productFavoriteRepository.AddAsync(favorites);
		}

		public async Task DeleteProductFavorite(string userId, long productId)
		=> await _productFavoriteRepository.DeleteAsync(userId, productId);

		public async Task LockOut(string userId, DateTimeOffset? endDate)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				if (endDate != null)
				{
					user.LockoutEnd = endDate.Value.AddDays(1);
				}
				else user.LockoutEnd = endDate;

				await _userManager.UpdateAsync(user);
			}
			else throw new ArgumentException($"Id {userId} " + ErrorMessage.NOT_FOUND);
		}

		public async Task<UserDTO> UpdateAvatar(string userId, IFormFile image)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				if (user.ImageURL != null)
				{
					_fileStorage.Delete(user.ImageURL);
				}
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
				user.ImageURL = Path.Combine(path, fileName);

				await _fileStorage.SaveAsync(path, image, fileName);
				await _userManager.UpdateAsync(user);
				return _mapper.Map<UserDTO>(user);
			}
			else { throw new ArgumentException(ErrorMessage.NOT_FOUND); }
		}

		public async Task<ImageDTO> GetAvatar(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				return _mapper.Map<ImageDTO>(user);
			}
			else
			{
				throw new ArgumentException(ErrorMessage.NOT_FOUND);
			}

		}


	}
}
