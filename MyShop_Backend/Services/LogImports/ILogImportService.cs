using MyShop_Backend.DTO;
using MyShop_Backend.Request;

namespace MyShop_Backend.Services.LogImports
{
	public interface ILogImportService
	{
		Task<ImportDTO> CreatedLog(LogImportRequest request);
	}
}
