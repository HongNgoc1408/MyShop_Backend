using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.BrandServices;

namespace MyShop_Backend.Controllers
{
	[Route("api/brands")]
	[ApiController]
	public class BrandsController(IBrandService brandService) : ControllerBase
	{
		private readonly IBrandService _brandService = brandService;

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				return Ok(await _brandService.GetBrandsAsync());
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("create")]
		[Authorize(Roles = "Admin,Inventorier")]
		
		public async Task<IActionResult> Create([FromForm] NameRequest request, [FromForm] IFormFileCollection image)
		{
			try
			{
				var brand = await _brandService.AddBrandAsync(request.Name, image.First());
				return Ok(brand);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("update/{id}")]
		[Authorize(Roles = "Admin,Inventorier")]
		public async Task<IActionResult> Update(int id, [FromForm] NameRequest request, [FromForm] IFormCollection files)
		{
			try
			{
				var image = files.Files.FirstOrDefault();
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

		[HttpDelete("delete/{id}")]
		[Authorize(Roles = "Admin,Inventorier")]
		public async Task<IActionResult> Delete(int id)
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
