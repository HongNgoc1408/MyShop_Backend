using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.ImportDetailRepositories;
using MyShop_Backend.Repositories.ImportRepositories;
using MyShop_Backend.Repositories.ProductColorRepositories;
using MyShop_Backend.Repositories.ProductSizeRepositories;
using MyShop_Backend.Request;
using MyShop_Backend.Response;
using MyStore.Repository.ProductRepository;
using System.Globalization;
using System.Linq.Expressions;

namespace MyShop_Backend.Services.Imports
{
	public class ImportService : IImportService
	{
		private readonly IImportRepository _importRepository;
		private readonly IImportDetailRepository _importDetailRepository;
		private readonly IProductRepository _productRepository;
		private readonly IProductColorRepository _productColorRepository;
		private readonly IProductSizeRepository _productSizeRepository;
		private readonly IMapper _mapper;

		public ImportService(IImportRepository importRepository, IImportDetailRepository importDetailRepository, IProductRepository productRepository, IProductColorRepository productColorRepository, IProductSizeRepository productSizeRepository, IMapper mapper)
		{
			_importRepository = importRepository;
			_importDetailRepository = importDetailRepository;
			_productRepository = productRepository;
			_productSizeRepository = productSizeRepository;
			_productColorRepository = productColorRepository;
			_mapper = mapper;
		}
		public async Task<ImportDTO> CreateImport(string userId, ImportRequest request)
		{
			try
			{
				var import = new Import
				{
					UserId = userId,
					Note = request.Note,
					Total = request.Total,
					EntryDate = DateTime.Now,
				};
				await _importRepository.AddAsync(import);

				var listProductUpdate = new List<Product>();
				var listProductSizeUpdate = new List<ProductSize>();
				var listImportDetail = new List<ImportDetail>();

				foreach (var item in request.ImportProducts)
				{
					var product = await _productSizeRepository.SingleAsyncInclude(e => e.ProductColorId == item.ColorId && e.SizeId == item.SizeId);
					if (product != null)
					{
						product.InStock += item.Quantity;
						listProductSizeUpdate.Add(product);
						
						var importDetail = new ImportDetail
						{
							ImportId = import.Id,
							ProductId = item.ProductId,
							ColorId = item.ColorId,
							ColorName = product.ProductColor.ColorName,
							SizeId = item.SizeId,
							SizeName = product.Size.Name,
							Quantity = item.Quantity,
							Price = item.Price,


						};
						listImportDetail.Add(importDetail);
					}
				}
				await _productSizeRepository.UpdateAsync(listProductSizeUpdate);
				await _importDetailRepository.AddAsync(listImportDetail);

				return _mapper.Map<ImportDTO>(import);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<PagedResponse<ImportDTO>> GetAll(int page, int pageSize, string? search)
		{
			int total;
			IEnumerable<Import> import;

			if (string.IsNullOrEmpty(search))
			{
				total = await _importRepository.CountAsync();
				import = await _importRepository.GetPagedOrderByDescendingAsync(page, pageSize, null, x => x.CreatedAt);
			}
			else
			{
				bool isLong = long.TryParse(search, out long isSearch);
				DateTime dateSearch;
				bool isDate = DateTime.TryParseExact(
					search,
					"HH:mm:ss dd/MM/yyyy",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out dateSearch);

				Expression<Func<Import, bool>> expression = e => e.Id.Equals(isSearch) || (!isLong && isDate && e.CreatedAt.Date == dateSearch.Date);

				total = await _importRepository.CountAsync(expression);
				import = await _importRepository.GetPagedOrderByDescendingAsync(page, pageSize, expression, e => e.CreatedAt);
			}

			var items = _mapper.Map<IEnumerable<ImportDTO>>(import);
			return new PagedResponse<ImportDTO>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalItems = total
			};

		}

		public async Task<IEnumerable<ImportDetailResponse>> GetDetails(long id)
		{
			var import = await _importDetailRepository.GetAsync(e=> e.ImportId == id);
			if(import != null)
			{
				var res = _mapper.Map<IEnumerable<ImportDetailResponse>>(import);
				//foreach (var item in res)
				//{
				//	item.ColorName = (await _productColorRepository.SingleOrDefaultAsync(e => e.Id == item.ColorId))?.ColorName ?? "";
				//}

				return res;
			}
			else throw new InvalidOperationException(ErrorMessage.NOT_FOUND);
		}
	}
}
