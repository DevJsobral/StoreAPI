using AutoMapper;
using TrainingAPI.DTOs.OrderDTOs;
using TrainingAPI.Models;

namespace TrainingAPI.DTOs.Mapping;

public class DTOMappingProfile : Profile
{
    public DTOMappingProfile()
    {
        CreateMap<Product, ProductDTORequest>().ReverseMap();
        CreateMap<Product, PutProductDTORequest>().ReverseMap();
        CreateMap<Product, ProductDTOResponse>().ReverseMap();
        CreateMap<Product, ProductPatchDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Category, PutCategoryDTORequest>().ReverseMap();
        CreateMap<Category, CategoryDTOResponse>().ReverseMap();
        CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
        CreateMap<OrderRequestDTO, Order>().ReverseMap();

        CreateMap<Order, OrderResponseDTO>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<OrderItem, OrderItemResponseDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));

    }
}
