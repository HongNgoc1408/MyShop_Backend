using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.CategoryService;

namespace MyShop_Backend.Controllers
{
	[Route("api/category")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService) => _categoryService = categoryService;

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var category = await _categoryService.GetAllCategoriesAsync();
				return Ok(category);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("get/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var category = await _categoryService.GetByIdCategoryAsync(id);
				return Ok(category);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("create")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] NameRequest request)
		{
			try
			{
				var category = await _categoryService.AddCategoryAsync(request.Name);
				return Ok(category);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("update/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(int id, [FromBody] NameRequest request)
		{
			try
			{
				var result = await _categoryService.UpdateCategoryAsync(id, request.Name);
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
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _categoryService.DeleteCategoryAsync(id);
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
