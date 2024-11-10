using MyShop_Backend.DTO;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.LogImports
{
	public interface ILogImportService
	{
		Task<ImportDTO> CreatedLog(LogImportRequest request);
		Task<PagedResponse<LogImportDTO>> GetAll(int page, int pageSize, string? key);
		Task<IEnumerable<ImportDetailResponse>> GetById(long id);
	}
}
