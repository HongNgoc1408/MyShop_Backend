using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Request;
using MyShop_Backend.Response;
using MyShop_Backend.Services.CachingServices;
using MyShop_Backend.Services.SendMailServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyShop_Backend.Services.AuthServices
{
	public class AuthService : IAuthService
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _config;
		private readonly ISendMailService _emailSender;
		private readonly ICachingService _cachingService;
		private readonly IMapper _mapper;

		public AuthService(
			SignInManager<User> signInManager,
			UserManager<User> userManager,
			IConfiguration config,
			ISendMailService emailSender,
			ICachingService cachingService,
			IMapper mapper
			)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_config = config;
			_emailSender = emailSender;
			_cachingService = cachingService;
			_mapper = mapper;
		}

		private async Task<string> CreateJWT(User user, bool isRefreshToken = false)
		{
			var roles = await _userManager.GetRolesAsync(user);
			var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Email, user.Email ?? ""),
					new Claim(ClaimTypes.Name, user.FullName ?? "")
				};
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			if (isRefreshToken)
			{
				claims.Add(new Claim(ClaimTypes.Version, "Refresh Token"));
			}

			var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Key"] ?? ""));
			var jwtToken = new JwtSecurityToken(
					issuer: _config["JWT:Issuer"],
					audience: _config["JWT:Audience"],
					claims: claims,
					expires: DateTime.Now.AddHours(12),
					signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
			return new JwtSecurityTokenHandler().WriteToken(jwtToken);
		}
		public async Task<JwtResponse> Login(LoginRequest request)
		{
			var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
			if (result.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync(request.Username);
				if (user != null)
				{
					var roles = await _userManager.GetRolesAsync(user);
					var accessToken = await CreateJWT(user);
					var refreshToken = await CreateJWT(user, true);

					return new JwtResponse
					{
						Access_token = accessToken,
						Refresh_token = refreshToken,
						FullName = user.FullName,
						Email = user.Email,
						PhoneNumber = user.PhoneNumber,
						Roles = roles,

					};
				}
				throw new Exception(ErrorMessage.USER_NOT_FOUND);
			}
			throw new Exception(ErrorMessage.INCORRECT_PASSWORD);
		}

		public async Task<IdentityResult> Register(RegisterRequest request)
		{
			var isTokenValid = VerifyResetToken(request.Email, request.Token);

			if (isTokenValid)
			{
				var user = new User()
				{
					Email = request.Email,
					NormalizedEmail = request.Email,
					UserName = request.Email,
					NormalizedUserName = request.Email,
					PhoneNumber = request.PhoneNumber,
					FullName = request.FullName,
					SecurityStamp = Guid.NewGuid().ToString(),
					ConcurrencyStamp = Guid.NewGuid().ToString()
				};

				var result = await _userManager.CreateAsync(user, request.Password);
				if (!result.Succeeded)
				{
					throw new Exception(ErrorMessage.INVALID);
				}
				await _userManager.AddToRoleAsync(user, "User");
				return IdentityResult.Success;
			}
			else throw new Exception("Invalid reset token.");
		}

		public async Task Logout(string userID)
		{
			var user = await _userManager.FindByIdAsync(userID);
			if (user == null)
			{
				throw new ArgumentException(ErrorMessage.USER_NOT_FOUND); 
			}
			var provider = "MyShop_Backend";
			var name = "Refresh_Token";
			await _userManager.RemoveAuthenticationTokenAsync(user, provider, name);
			await _userManager.UpdateSecurityStampAsync(user);
			await _signInManager.SignOutAsync();
		}

		public async Task<bool> SendPasswordResetTokenAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return false;
			}

			var token = new Random().Next(100000, 999999).ToString();
			_cachingService.Set(email, token, TimeSpan.FromMinutes(5));

			var message = $"Mã xác thực khôi phục mật khẩu là: {token}";
			await _emailSender.SendEmailAsync(email, "Mã xác thực khôi phục mật khẩu", message);

			return true;
		}

		public async Task<bool> SendTokenAsync(string email)
		{
			var token = new Random().Next(100000, 999999).ToString();
			_cachingService.Set(email, token, TimeSpan.FromMinutes(5));

			var message = $"Mã xác nhận tạo tài khoản MyShop: {token}";
			await _emailSender.SendEmailAsync(email, "Mã xác thực", message);

			return true;
		}

		public bool VerifyResetToken(string email, string token)
		{
			var cachedToken = _cachingService.Get<string>(email);
			if (cachedToken == null || cachedToken != token) return false;
			else return true;
		}

		public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
		{
			var isTokenValid = VerifyResetToken(email, token);
			if (!isTokenValid) return false;

			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return false;
			}

			var t = await _userManager.GeneratePasswordResetTokenAsync(user);

			var result = await _userManager.ResetPasswordAsync(user, t, newPassword);
			await _userManager.UpdateSecurityStampAsync(user);
			_cachingService.Remove(email);
			return result.Succeeded;
		}

		//public Task<Token> RefreshToken(Token token)
		//{
		//	throw new NotImplementedException();
		//}

	}
}