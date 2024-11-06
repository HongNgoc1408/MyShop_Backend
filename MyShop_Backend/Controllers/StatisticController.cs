using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MyShop_Backend.Services.Statistices;

namespace MyShop_Backend.Controllers
{
	[Route("api/statistic")]
	[ApiController]
	public class StatisticController(IStatisticService statisticService) : ControllerBase
	{
		private readonly IStatisticService _statisticService = statisticService;

		[HttpGet("totalImport")]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> GetTotalImport()
		{
			try
			{
				var res = await _statisticService.GetCountImport();
				return Ok(res);
			}
			catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("totalOrder")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalOrder() {
			try
			{
				var res = await _statisticService.GetCountOrder();
				return Ok(res);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("totalOrderDone")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalOrderDone()
		{
			try
			{
				var res = await _statisticService.GetCountOrderDone();
				return Ok(res);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("totalProduct")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalProduct()
		{
			try
			{
				var res = await _statisticService.GetCountProduct();
				return Ok(res);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("totalUser")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalUser()
		{
			try
			{
				var res = await _statisticService.GetCountUser();
				return Ok(res);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("totalSpending-by-year")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalSpendingByYear(int year, int? month)
		{
			try
			{
				var result = await _statisticService.GetTotalSpendingByYear(year, month);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("totalSpending")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalSpending(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				var res = await _statisticService.GetTotalSpending(dateFrom, dateTo);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("totalSold-by-year")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalSoldByYear(int year, int? month)
		{
			try
			{
				var result = await _statisticService.GetTotalSoldByYear(year, month);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("totalSold")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalSold(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				var res = await _statisticService.GetTotalSold(dateFrom, dateTo);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("totalRevenue")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTotalRevenue(int year, int? month)
		{
			try
			{
				var res = await _statisticService.GetRevenue(year, month);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
