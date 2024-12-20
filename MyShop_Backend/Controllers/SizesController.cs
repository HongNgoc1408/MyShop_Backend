﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.Sizes;

namespace MyShop_Backend.Controllers
{
	[Route("api/sizes")]
	[ApiController]
	public class SizesController(ISizeService sizeService) : ControllerBase
	{
		private readonly ISizeService _sizeService = sizeService;

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				return Ok(await _sizeService.GetSizesAsync());
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(long id)
		{
			try
			{
				var size = await _sizeService.GetByIdSizeAsync(id);
				return Ok(size);
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

		[HttpPost("create")]
		[Authorize(Roles = "Admin,Inventorier")]
		public async Task<IActionResult> Create([FromBody] NameRequest request)
		{
			try
			{
				var size = await _sizeService.AddSizeAsync(request.Name);
				return Ok(size);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("update/{id}")]
		[Authorize(Roles = "Admin,Inventorier")]
		public async Task<IActionResult> Update(long id, [FromBody] NameRequest request)
		{
			try
			{
				var size = await _sizeService.UpdateSizeAsync(id, request.Name);
				return Ok(size);
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

		[HttpDelete("delete/{id}")]
		[Authorize(Roles = "Admin,Inventorier")]
		public async Task<IActionResult> Delete(long id)
		{
			try
			{
				await _sizeService.DeleteSizeAsync(id);
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
}
