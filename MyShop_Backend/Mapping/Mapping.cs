using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.Models;

namespace MyShop_Backend.Mappers
{
	public class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<CategoryDTO, Category>().ReverseMap();  // ReverseMap hai chieu
		}
	}
}
