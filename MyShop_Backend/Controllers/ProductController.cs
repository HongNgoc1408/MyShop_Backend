using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.ProductServices;

namespace MyShop_Backend.Controllers
{
	[Route("api/product")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService) => _productService = productService;

		[HttpGet]
		public async Task<IActionResult> GetAllProduct([FromQuery] PagedRequest request)
		{
			try
			{
				return Ok(await _productService.GetAllProductAsync(request.page, request.pageSize, request.search));
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var result = await _productService.GetProductById(id);
				return Ok(result);
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

		[HttpPost("add")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddProduct([FromForm] ProductRequest request, [FromForm] IFormFileCollection form)
		{
			try
			{
				var images = form;
				var product = await _productService.AddProductAsync(request, images);
				return Ok(product);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("update/{id}")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(int id, [FromForm] ProductRequest request, [FromForm] IFormCollection form)
		{
			try
			{
				var images = form.Files;
				var result = await _productService.UpdateProduct(id, request, images);
				return Ok(result);
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
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _productService.DeleteProductAsync(id);
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
