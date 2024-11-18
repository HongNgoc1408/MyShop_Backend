using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.Carts;
using System.Security.Claims;

namespace MyShop_Backend.Controllers
{
	[Route("api/cart")]
	[ApiController]
	[Authorize]
	public class CartController(ICartService cartService) : ControllerBase
	{
		private readonly ICartService _cartService = cartService;

		[HttpGet]
		public async Task<IActionResult> Cart()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var cartItems = await _cartService.GetAllByUserId(userId);
				return Ok(cartItems);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

		}

		[HttpGet("count")]
		public async Task<IActionResult> GetCount()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var carts = await _cartService.GetCountProdutctId(userId);
				return Ok(carts);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] CartRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				await _cartService.AddToCart(userId, request);
				return Ok(request);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

		}

		[HttpPut("update/{id}")]
		public async Task<IActionResult> Update(string id, [FromBody] UpdateCartItemRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				await _cartService.UpdateCartItem(id, userId, request);
				return Ok(request);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

		}

		[HttpDelete("delete")]
		public async Task<IActionResult> Delete([FromQuery] IEnumerable<long> productId)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				await _cartService.DeleteCartAsync(userId, productId);
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

		[HttpDelete("{id}")]
		public async Task<IActionResult> Detete(string id)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				await _cartService.DeleteCartItem(id, userId);
				return Ok();
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
