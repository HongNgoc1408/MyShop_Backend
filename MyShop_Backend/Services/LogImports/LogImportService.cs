using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.LogDetailRepositories;
using MyShop_Backend.Repositories.LogRepositories;
using MyShop_Backend.Request;
using MyShop_Backend.Response;
using MyShop_Backend.Services.LogImports;

namespace MyShop_Backend.Services.Log
{
	public class LogImportService : ILogImportService
	{
		private readonly IMapper _mapper;
		private readonly ILogImportDetailRepository _logImportDetailRepository;
		private readonly ILogImportRepository _logImportRepository;

		public LogImportService(IMapper mapper, ILogImportRepository logImportRepository, ILogImportDetailRepository logImportDetailRepository)
		{
			_mapper = mapper;
			_logImportDetailRepository = logImportDetailRepository;
			_logImportRepository = logImportRepository;
		}
		public async Task<ImportDTO> CreatedLog(LogImportRequest request)
		{
			var log = new LogImport
			{
				UserId = request.UserId,
				Note = request.Note,
				Total = request.Total,
				EntryDate = request.EntryDate,
				ImportId = request.ImportId,
			};

			await _logImportRepository.AddAsync(log);

			var listLogImportDetail = new List<LogImportDetail>();

			foreach (var item in request.LogImportProducts)
			{

				var logDetail = new LogImportDetail
				{
					ProductName = item.ProductName,
					ColorName = item.ColorName,
					SizeName = item.SizeName,
					Price = item.Price,
					Quantity = item.Quantity,
					LogId = log.Id,

				};
				listLogImportDetail.Add(logDetail);
			}
			await _logImportDetailRepository.AddAsync(listLogImportDetail);

			return _mapper.Map<ImportDTO>(log);
		}

		public async Task<PagedResponse<LogImportDTO>> GetAll(int page, int pageSize, string? key)
		{
			int total = 0;
			IEnumerable<LogImport> logImports;
			if (string.IsNullOrEmpty(key))
			{
				total = await _logImportRepository.CountAsync();
				logImports = await _logImportRepository.GetPagedOrderByDescendingAsync(page, pageSize, null, e => e.CreatedAt);
			}
			else
			{
				total = await _logImportRepository.CountAsync();
				logImports = await _logImportRepository.GetPagedOrderByDescendingAsync(page, pageSize, e => e.ImportId == long.Parse(key), e => e.CreatedAt);
			}

			var items = _mapper.Map<IEnumerable<LogImportDTO>>(logImports);
			return new PagedResponse<LogImportDTO>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = total
			};
		}

		public async Task<IEnumerable<ImportDetailResponse>> GetById(long id)
		{
			var log = await _logImportDetailRepository.GetAsync(e => e.LogId == id);
			if (log != null)
			{
				return _mapper.Map<IEnumerable<ImportDetailResponse>>(log);
			}
			else throw new InvalidOperationException(ErrorMessage.NOT_FOUND);
		}
	}
}
