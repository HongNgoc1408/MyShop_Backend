﻿using Microsoft.AspNetCore.Authorization;
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
	}
};
