using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop_Backend.Request;
using MyShop_Backend.Services.Log;
using MyShop_Backend.Services.LogImports;

namespace MyShop_Backend.Controllers
{
	[Route("api/logImport")]
	[ApiController]
	[Authorize]
	public class LogImportController(ILogImportService logImportService) : ControllerBase
	{
		private readonly ILogImportService _logImportService = logImportService;

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll([FromQuery] PageRequest request)
		{
			try
			{
				var res = await _logImportService.GetAll(request.Page, request.PageSize, request.Key);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetById(long id)
		{
			try
			{
				var res = await _logImportService.GetById(id);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
