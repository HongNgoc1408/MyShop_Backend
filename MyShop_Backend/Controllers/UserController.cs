using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.UserServices;

namespace MyShop_Backend.Controllers
{
	[Route("api/user")]
	[ApiController]
	[Authorize]
	public class UserController(IUserService userService) : ControllerBase
	{
		private readonly IUserService _userService = userService;
		
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllUser([FromQuery] PageRequest request)
		{
			try
			{
				var users = await _userService.GetAllUserAsync(request.Page, request.PageSize, request.Key);
				return Ok(users);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
