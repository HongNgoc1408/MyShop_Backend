using Microsoft.AspNetCore.Identity;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.AuthServices
{
	public interface IAuthService
	{
		Task<JwtResponse> Login(LoginRequest request);
		Task<IdentityResult> Register(RegisterRequest request);

		//Task<Token> RefreshToken(Token token);
		Task Logout(string userID);
		Task<bool> SendPasswordResetTokenAsync(string email);
		bool VerifyResetToken(string email, string token);
		Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
		Task<bool> SendTokenAsync(string email);
	}
}
