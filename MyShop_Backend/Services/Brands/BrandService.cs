using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.ErroMessage;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.BrandRepositories;
using MyShop_Backend.Storages;

namespace MyShop_Backend.Services.BrandServices
{
	public class BrandService : IBrandService
	{
		private readonly string path = "assets/images/brands";
		private readonly IBrandRepository _brandRepository;
		private readonly IMapper _mapper;
		private readonly IFileStorage _fileStorage;

		public BrandService(IBrandRepository brandRepository, IMapper mapper, IFileStorage fileStorage)
		{
			_brandRepository = brandRepository;
			_mapper = mapper;
			_fileStorage = fileStorage;
		}

		public async Task<BrandDTO> AddBrandAsync(string name, IFormFile image)
		{
			try
			{
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

				Brand brand = new()
				{
					Name = name,
					ImageUrl = Path.Combine(path, fileName)
				};

				await _brandRepository.AddAsync(brand);
				await _fileStorage.SaveAsync(path, image, fileName);
				return _mapper.Map<BrandDTO>(brand);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task DeleteBrandAsync(int id)
		{
			try
			{
				var brand = await _brandRepository.FindAsync(id);
				if (brand != null)
				{
					_fileStorage.Delete(brand.ImageUrl);
					await _brandRepository.DeleteAsync(id);
				}
				else throw new Exception($"ID {id} " + ErrorMessage.NOT_FOUND);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task<IEnumerable<BrandDTO>> GetAllBrandAsync()
		{
			var brands = await _brandRepository.GetAllAsync();
			var sortedBrands = brands.OrderBy(b => b.Name); 
			return _mapper.Map<IEnumerable<BrandDTO>>(sortedBrands);
		}


		public async Task<BrandDTO> GetByIdBrandAsync(int id)
		{
			var brand = await _brandRepository.FindAsync(id);

			if (brand == null)
			{
				throw new ArgumentException($"ID {id}"
					+ ErrorMessage.NOT_FOUND);
			}
			else
			{
				return _mapper.Map<BrandDTO>(brand);
			}
		}

		public async Task<BrandDTO> UpdateBrandAsync(int id, string name, IFormFile? image)
		{
			var brand = await _brandRepository.FindAsync(id);
			if (brand == null)
			{
				throw new ArgumentException($"ID {id}"
					+ ErrorMessage.NOT_FOUND);
			}
			else
			{
				brand.Name = name;
				if (image != null)
				{
					_fileStorage.Delete(brand.ImageUrl);

					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
					brand.ImageUrl = Path.Combine(path, fileName);

					await _fileStorage.SaveAsync(path, image, fileName);
				}
				await _brandRepository.UpdateAsync(brand);
				await _brandRepository.UpdateAsync(brand);
				return _mapper.Map<BrandDTO>(brand);
			}
		}
	}
}
