using AutoMapper;
using Webminux.Optician.ProductGroups.Dto;
using Webminux.Optician.Products.Dtos;

namespace Webminux.Optician.Products.Dto
{
    /// <summary>
    /// Defines a mapping between the <see cref="Product"/> and <see cref="ProductDto"/>.
    /// </summary>
    public class ProductGroupMapProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductGroupMapProfile"/> class.
        /// </summary>
        public ProductGroupMapProfile()
        {
            CreateMap<ProductGroupDto, ProductGroup>();
            CreateMap<ProductGroup, ProductGroupDto>();
            CreateMap<ProductGroupDto, ProductGroup>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(g => g.CreationTime, opt => opt.Ignore())
                .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

            CreateMap<ProductGroupDto, ProductGroup>()
                  .ForMember(g => g.CreationTime, opt => opt.Ignore())
                    .ForMember(g => g.CreatorUserId, opt => opt.Ignore());


        }
    }
}
