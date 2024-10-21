using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyShop_Backend.DTO;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.DeliveryAddressRepositories;
using MyShop_Backend.Repositories.ProductFavoriteRepositories;
using MyShop_Backend.Repositories.UserRepositories;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.UserServices
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly IUserRepository _userRepository;
		private readonly IDeliveryAddressRepository _deliveryAddressRepository;
		private readonly IProductFavoriteRepository _productFavoriteRepository;
		private readonly IMapper _mapper;

		public UserService(UserManager<User> userManager, IUserRepository userRepository, IDeliveryAddressRepository deliveryAdressRepository, IProductFavoriteRepository productFavoriteRepository, IMapper mapper)
		{
			_userManager = userManager;
			_userRepository = userRepository;
			_deliveryAddressRepository = deliveryAdressRepository;
			_productFavoriteRepository = productFavoriteRepository;
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
	}
}
