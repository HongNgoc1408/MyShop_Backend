using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.BrandServices;
using MyShop_Backend.Services.CategoryService;

namespace MyShop_Backend.Controllers
{
	[Route("api/brand")]
	[ApiController]
	public class BrandController : ControllerBase
	{
		private readonly IBrandService _brandService;

		public BrandController(IBrandService brandService) => _brandService = brandService;

		[HttpGet]
		public async Task<IActionResult> GetAllBrand()
		{
			try
			{
				var brand = await _brandService.GetAllBrandAsync();
				return Ok(brand);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("add")]
		public async Task<IActionResult> AddBrand([FromForm] NameRequest request, [FromForm] IFormFileCollection files)
		{
			try
			{
				var image = files.First();
				var brand = await _brandService.AddBrandAsync(request.Name, image);
				return Ok(brand);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("update/{id}")]
		public async Task<IActionResult> UpdateBrand(int id, [FromForm]  NameRequest request,  IFormFile files)
		{
			try
			{
				var image = files;
				var brand = await _brandService.UpdateBrandAsync(id, request.Name, image);
				return Ok(brand);
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

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdBrand(int id)
		{
			try
			{
				var brand = await _brandService.GetByIdBrandAsync(id);
				return Ok(brand);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteBrand(int id)
		{
			try
			{	
				await _brandService.DeleteBrandAsync(id);
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
