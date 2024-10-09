using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Mappers
{
	public class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<User, UserResponse>().ReverseMap();
			CreateMap<Category, CategoryDTO>().ReverseMap();
			CreateMap<Brand, BrandDTO>().ReverseMap();
			CreateMap<ProductRequest, Product>().ReverseMap();
			CreateMap<Product, ProductDTO>().ReverseMap();
			CreateMap<Product, ProductDTO>()
				.ForMember(des => des.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
				.ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.Caterory.Name));

			CreateMap<ProductRequest, Product>();
			CreateMap<Product, ProductDetailResponse>();
			CreateMap<ImageDTO, Image>().ReverseMap();
			CreateMap<SizeDTO, Size>().ReverseMap();
			CreateMap<TokenDTO, Token>().ReverseMap();
		}
	}
}
