using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.LogDetailRepositories;
using MyShop_Backend.Repositories.LogRepositories;
using MyShop_Backend.Request;
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
	}
}
