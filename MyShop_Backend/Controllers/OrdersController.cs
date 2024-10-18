using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.Orders;
using System.Security.Claims;

namespace MyShop_Backend.Controllers
{
	[Route("api/orders")]
	[ApiController]
	[Authorize]
	public class OrdersController(IOrderService orderService) : ControllerBase
	{
		private readonly IOrderService _orderService = orderService;

		//Admin
		[HttpGet("get-all")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll([FromQuery] PageRequest request)
		{
			try
			{
				return Ok(await _orderService.GetAll(request.Page, request.PageSize, request.Key));
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		

		[HttpPut("updateStatus/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusOrderRequest request)
		{
			try
			{
				var orders = await _orderService.UpdateOrder(id, request);
				return Ok(orders);
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
		public async Task<IActionResult> Delete(long id)
		{
			try
			{
				await _orderService.DeleteOrder(id);
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

		[HttpPut("shipping/{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Shipping(long orderId, [FromBody] OrderToShippingRequest request)
        {
            try
            {
                await _orderService.OrderToShipping(orderId, request);
                return Ok();
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

		///User
		[HttpGet]
		public async Task<IActionResult> GetOrdersByUserId([FromQuery] PageRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var orders = await _orderService.GetOrdersByUserId(userId, request);
				return Ok(orders);
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
		public async Task<IActionResult> OrderDetails(long id)
		{
			try
			{
				var roles = User.FindAll(ClaimTypes.Role).Select(e => e.Value);
				var isAdmin = roles.Contains("Admin");

				if (isAdmin)
				{
					var orders = await _orderService.GetOrderDetail(id);
					return Ok(orders);
				}
				else
				{
					var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
					if (userId == null)
					{
						return Unauthorized();
					}
					var orders = await _orderService.GetOrderDetail(id, userId);
					return Ok(orders);
				}
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] OrderRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var orders = await _orderService.CreateOrder(userId, request);
				return Ok(orders);
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

		[HttpPut("update/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				var orders = await _orderService.UpdateOrder(id, userId, request);
				return Ok(orders);
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

		
		[HttpDelete("cancel/{id}")]
		public async Task<IActionResult> Cancel(int id)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				await _orderService.CancelOrder(id, userId);
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
