using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop_Backend.DTO;
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

		[HttpPut("lock-out/{id}")]
		[Authorize(Roles = "Admin")]
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
