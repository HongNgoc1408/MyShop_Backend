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
	public class LogService : ILogImportService
	{
		private readonly IMapper _mapper;
		private readonly ILogDetailRepository _logDetailRepository;
		private readonly ILogRepository _logRepository;

		public LogService(IMapper mapper, ILogRepository logRepository, ILogDetailRepository logDetailRepository)
		{
			_mapper = mapper;
			_logDetailRepository = logDetailRepository;
			_logRepository = logRepository;
		}
		public async Task<ImportDTO> CreatedLog(LogRequest request)
		{
			var log = new Models.Log
			{
				UserId = request.UserId,
				Note = request.Note,
				Total = request.Total,
				EntryDate = request.EntryDate,
				ImportId = request.ImportId,
			};

			await _logRepository.AddAsync(log);

			var listLogDetail = new List<LogDetail>();

			foreach (var item in request.LogImportProducts)
			{

				var logDetail = new LogDetail
				{
					ProductName = item.ProductName,
					ColorName = item.ColorName,
					SizeName = item.SizeName,
					Price = item.Price,
					Quantity = item.Quantity,
					LogId = log.Id,

				};
				listLogDetail.Add(logDetail);
			}
			await _logDetailRepository.AddAsync(listLogDetail);

			return _mapper.Map<ImportDTO>(log);
		}

		public async Task<PagedResponse<LogDTO>> GetAll(int page, int pageSize, string? key)
		{
			int total = 0;
            IEnumerable<Models.Log> logImports;
			if (string.IsNullOrEmpty(key))
			{
				total = await _logRepository.CountAsync();
				logImports = await _logRepository.GetPagedOrderByDescendingAsync(page, pageSize, null, e => e.CreatedAt);
			}
			else
			{
				total = await _logRepository.CountAsync();
				logImports = await _logRepository.GetPagedOrderByDescendingAsync(page, pageSize, e => e.ImportId == long.Parse(key), e => e.CreatedAt);
			}

			var items = _mapper.Map<IEnumerable<LogDTO>>(logImports);
			return new PagedResponse<LogDTO>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = total
			};
		}

		public async Task<IEnumerable<ImportDetailResponse>> GetById(long id)
		{
			var log = await _logDetailRepository.GetAsync(e => e.LogId == id);
			if (log != null)
			{
				return _mapper.Map<IEnumerable<ImportDetailResponse>>(log);
			}
			else throw new InvalidOperationException(ErrorMessage.NOT_FOUND);
		}
	}
}
