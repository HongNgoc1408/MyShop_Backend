using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.Imports;
using System.Security.Claims;

namespace MyShop_Backend.Controllers
{
	[Route("api/import")]
	[ApiController]
	public class ImportController(IImportService importService) : ControllerBase
	{
		private readonly IImportService _importService = importService;

		[HttpPost]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Create([FromBody] ImportRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return Unauthorized();
				}
				return Ok(await _importService.CreateImport(userId, request));
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll([FromQuery] PageRequest request)
		{
			var result = await _importService.GetAll(request.Page, request.PageSize, request.Key);
			return Ok(result);
		}

		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetStockDetail(long id)
		{
			try
			{
				var stockDetail = await _importService.GetDetails(id);
				return Ok(stockDetail);
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
	}
}
