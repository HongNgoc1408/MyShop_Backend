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
		[Authorize(Roles = "Admin,Inventorier,Staff")]

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
		[Authorize(Roles = "Admin,Inventorier,Staff")]
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
		[Authorize(Roles = "Admin,Inventorier,Staff")]
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
		[Authorize(Roles = "Admin,Inventorier,Staff")]
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
		[Authorize(Roles = "Admin,Inventorier,Staff")]
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

		[HttpGet("totalRevenueYear")]
		[Authorize(Roles = "Admin,Inventorier,Staff")]
		public async Task<IActionResult> GetTotalRevenueYear(int year, int? month)
		{
			try
			{
				var res = await _statisticService.GetRevenueByYear(year, month);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("totalRevenue")]
		[Authorize(Roles = "Admin,Inventorier,Staff")]
		public async Task<IActionResult> GetTotalRevenue(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				var res = await _statisticService.GetRevenue(dateFrom, dateTo);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("totalRevenueProductYear")]
		[Authorize(Roles = "Admin,Inventorier,Staff")]
		public async Task<IActionResult> GetProductRevenueByYear(int year, int? month, long productId)
		{
			try
			{
				var res = await _statisticService.GetProductRevenueByYear(productId, year, month);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("totalRevenueProduct")]
		[Authorize(Roles = "Admin,Inventorier,Staff")]
		public async Task<IActionResult> GetProductRevenue(DateTime dateFrom, DateTime dateTo, long productId)
		{
			try
			{
				var res = await _statisticService.GetProductRevenue(productId, dateFrom, dateTo);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
