using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.DTO;
using MyShop_Backend.Enumerations;
using MyShop_Backend.Request;
using MyShop_Backend.Services.UserServices;
using System.Security.Claims;

namespace MyShop_Backend.Controllers
{
	[Route("api/user")]
	[ApiController]
	[Authorize]
	public class UserController(IUserService userService) : ControllerBase
	{
		private readonly IUserService _userService = userService;

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] UserCreateDTO user)
		{
			try
			{
				var users = await _userService.AddUser(user);
				return Ok(users);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{userId}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(string userId, UpdateUserRequest request)
		{
			try
			{
				var user = await _userService.UpdateUser(userId,request);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("{userId}")]
		[Authorize(Roles = "Admin,Manage")]
		
		public async Task<IActionResult> Get(string userId)
		{
			try
			{
				var user = await _userService.GetUser(userId);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpGet]
		[Authorize(Roles = "Admin,Manage")]
		public async Task<IActionResult> GetAll([FromQuery] PageRequest request, RolesEnum role)
		{
			try
			{
				var users = await _userService.GetAllAsync(request.Page, request.PageSize, request.Key, role);
				return Ok(users);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("lock-out/{id}")]
		[Authorize(Roles = "Admin,Manage")]
		public async Task<IActionResult> LockOut(string id, [FromBody] LockOutRequest request)
		{
			try
			{
				await _userService.LockOut(id, request.EndDate);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpGet("address")]
		public async Task<IActionResult> GetAddress()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var address = await _userService.GetUserAddress(userId);
				return Ok(address);
			}
			catch (ArgumentException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("address")]
		public async Task<IActionResult> UpdateAddress([FromBody] AddressDTO address)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var result = await _userService.UpdateUserAddress(userId, address);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("profile")]
		public async Task<IActionResult> GetProfile()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if(userId == null)
				{
					return Unauthorized();
				}
				var address = await _userService.GetUserInfo(userId);
				return Ok(address);
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(ex.Message);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("avatar")]
		public async Task<IActionResult> GetAvatar()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var result = await _userService.GetAvatar(userId);
				return Ok(result);

			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("avatar")]
		public async Task<IActionResult> UpdateAvatar([FromForm] IFormCollection file )
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var image = file.Files.FirstOrDefault();
				var result = await _userService.UpdateAvatar(userId, image);
				return Ok(result);

			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("favorite")]
		public async Task<IActionResult> GetFavorite()
		{
			try {
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if( userId == null)
				{
					return Unauthorized();
				}
				var favorites = await _userService.GetFavorites(userId);
				return Ok(favorites);
			}catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("favorite/products")]
		public async Task<IActionResult> ProductFavorites([FromQuery] PageRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var pFavorites = await _userService.GetProductsFavorite(userId, request);
				return Ok(pFavorites);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("favorite")]
		public async Task<IActionResult> AddFavorie([FromBody] IdRequest<long> request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if(userId == null)
				{
					return Unauthorized();
				}
				await _userService.AddProductFavorite(userId, request.Id);
				return Created();
			}
			catch (Exception ex) 
			{
				return StatusCode(500, ex.Message);
			}


		}

		[HttpDelete("favorite/{id}")]
		public async Task<IActionResult> DeleteFavorite(long id) {
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
					
				}
				await _userService.DeleteProductFavorite(userId, id);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

		}

		
	}
};
