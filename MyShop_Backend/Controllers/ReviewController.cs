using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.Products;
using MyShop_Backend.Services.Reviews;

namespace MyShop_Backend.Controllers
{
	[Route("api/reviews")]
	[ApiController]
	public class ReviewController(IReviewService reviewService) : ControllerBase
	{
		private readonly IReviewService _reviewService = reviewService;

		[HttpPut("updateEnable/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateEnable(string id, [FromBody] UpdateEnableRequest request)
		{
			try
			{
				var result = await _reviewService.UpdateEnable(id, request);
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

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteReview(string id)
		{
			try
			{
				await _reviewService.DeleteReview(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		

	}
}
