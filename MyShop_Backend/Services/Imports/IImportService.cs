using Microsoft.EntityFrameworkCore.Metadata;
using MyShop_Backend.DTO;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Imports
{
	public interface IImportService
	{
		Task<ImportDTO> CreateImport(string userId, ImportRequest request);
		Task<PagedResponse<ImportDTO>> GetAll(int page, int pageSize, string? search);

		Task<IEnumerable<ImportDetailResponse>> GetDetails(long Id);
	}
}
