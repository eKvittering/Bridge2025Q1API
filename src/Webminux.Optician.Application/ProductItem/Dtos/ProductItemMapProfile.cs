using AutoMapper;
using Webminux.Optician.ProductItem;

public class ProductItemMapProfile : Profile
{
    public ProductItemMapProfile()
    {
        CreateMap<ProductItemDto, ProductItem>();
        CreateMap<ProductItem, ProductItemDto>();
        CreateMap<CreateProductItemDto, ProductItem>()
        .ForMember(g => g.Id, opt => opt.Ignore());

        CreateMap<UpdateProductItemDto, ProductItem>();
    }
}