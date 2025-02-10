using AutoMapper;
using Webminux.Optician.Products.Dtos;

namespace Webminux.Optician.Products.Dto
{
    /// <summary>
    ///  product dto mapper
    /// </summary>
    public class ProductMapProfile : Profile
    {

        /// <summary>
        /// Provide Mappings of Product and Product DTO
        /// </summary>
        public ProductMapProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(p => p.PictureUrl, options => options.Ignore())
                .ForMember(p => p.PicturePublicId, options => options.Ignore())
                .ForMember(p => p.ProductSerials, options => options.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(p => p.Base64Picture, options => options.Ignore());

            CreateMap<ProductDto, Product>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(g => g.CreationTime, opt => opt.Ignore())
                .ForMember(p => p.ProductSerials, options => options.Ignore())
                .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

        }
    }
}
