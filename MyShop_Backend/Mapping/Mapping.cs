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
			CreateMap<User, UserResponse>();
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<DeliveryAddress, AddressDTO>().ReverseMap();
			//product
			CreateMap<Product, NameDTO>().ReverseMap();
			CreateMap<Product, ProductDTO>().ReverseMap();
			CreateMap<Product, ProductDTO>()
				.ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault() != null ? src.Images.FirstOrDefault()!.ImageUrl : null))
				.ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Caterory.Name))
				.ForMember(dest => dest.Rating, opt => opt.MapFrom(src => Math.Round(src.Rating, 1))); ;

			CreateMap<ProductRequest, Product>();

			CreateMap<Product, ProductDetailsResponse>()
				.ForMember(dest => dest.ColorSizes, opt => opt.MapFrom(src => src.ProductColors))
				.ForMember(dest => dest.Rating, opt => opt.MapFrom(src => Math.Round(src.Rating, 1))); ;

			CreateMap<ProductColor, ColorSizeResponse>()
				.ForMember(dest => dest.SizeInStocks, opt => opt.MapFrom(src => src.ProductSizes));

			CreateMap<ProductColor, ColorDTO>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ColorName));

			CreateMap<ProductSize, SizeInStock>()
				.ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name));

			CreateMap<ProductSize, SizeDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SizeId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Size.Name));

			//img
			CreateMap<ImageDTO, Image>().ReverseMap();

			//order
			CreateMap<OrderDTO, Order>().ReverseMap()
				.ForMember(d => d.PaymentMethodName, opt => opt.MapFrom(src => src.PaymentMethodName));

			CreateMap<OrderRequest, Order>().ReverseMap();
			CreateMap<OrderDetail, ProductOrderDetails>();

			CreateMap<Order, OrderDetailsResponse>()
				.ForMember(d => d.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethodName))
				.ForMember(dest => dest.ProductOrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

			//Payment Method
			CreateMap<PaymentMethod, PaymentMethodDTO>().ReverseMap();

			//Import 
			//CreateMap<Import, ImportDTO>().ReverseMap();
			CreateMap<Import, ImportDTO>()
				.ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null));

			CreateMap<ImportDetail, ImportDetailResponse>()
				.ForMember(d => d.ProductName, opt => opt.MapFrom(src => src.Product.Name));

			//review
			CreateMap<ProductReview, ReviewDTO>()
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null));
		}
	}
}
