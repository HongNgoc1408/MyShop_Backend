using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.SizeRepositories;

namespace MyShop_Backend.Services.Sizes
{
	public class SizeService : ISizeService
	{
		private readonly ISizeRepository _sizeRepository;
		private readonly IMapper _mapper;
		public SizeService(ISizeRepository sizeRepository, IMapper mapper)
		{
			_sizeRepository = sizeRepository;
			_mapper = mapper;
		}
		public async Task<IEnumerable<SizeDTO>> GetSizesAsync()
		{
			var sizes = await _sizeRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<SizeDTO>>(sizes);
		}

		public async Task<SizeDTO> AddSizeAsync(string name)
		{
			try
			{
				var size = new Size
				{
					Name = name
				};
				await _sizeRepository.AddAsync(size);

				return _mapper.Map<SizeDTO>(size);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task DeleteSizeAsync(long id)
		{
			try
			{
				await _sizeRepository.DeleteAsync(id);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<SizeDTO> UpdateSizeAsync(long id, string name)
		{
			var size = await _sizeRepository.FindAsync(id);
			if (size != null)
			{
				size.Name = name;
				await _sizeRepository.UpdateAsync(size);
				return _mapper.Map<SizeDTO>(size);
			}
			else throw new ArgumentException(ErrorMessage.NOT_FOUND);
		}
	}
}
