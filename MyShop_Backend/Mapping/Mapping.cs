using AutoMapper;
using MyShop_Backend.DTO;
using MyShop_Backend.Models;
using MyShop_Backend.ModelView;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Mappers
{
	public class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<TokenDTO, Token>().ReverseMap();

			CreateMap<Category, CategoryDTO>().ReverseMap();

			CreateMap<Brand, BrandDTO>().ReverseMap();

			CreateMap<SizeDTO, Size>().ReverseMap();


			//user
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<User, UserResponse>();
			CreateMap<DeliveryAddress, AddressDTO>().ReverseMap();
			//product
			CreateMap<Product, ProductDTO>().ReverseMap();
			CreateMap<Product, ProductDTO>()
				.ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault() != null ? src.Images.FirstOrDefault()!.ImageUrl : null))
				.ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Caterory.Name));
			CreateMap<ProductRequest, Product>();
			CreateMap<Product, ProductDetailsResponse>();

			CreateMap<ProductColor, ColorSizeResponse>()
				.ForMember(dest => dest.SizeInStocks, opt => opt.MapFrom(src => src.ProductSizes));

			CreateMap<ProductSize, SizeInStock>()
				.ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name));

			//img
			CreateMap<ImageDTO, Image>().ReverseMap();

			//order

		}
	}
}
