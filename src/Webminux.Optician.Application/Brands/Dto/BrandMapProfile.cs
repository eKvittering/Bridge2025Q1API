using AutoMapper;
using Webminux.Optician.Brands;

/// <summary>
/// barnd auto mapping
/// </summary>

public class BrandMapProfile : Profile
{
    /// <summary>
    ///  maping Brand
    /// </summary>
    public BrandMapProfile()
    {
        CreateMap<BrandDto, Brand>();
        CreateMap<Brand, BrandDto>();
        CreateMap<CreateGroupDto, Group>()
        .ForMember(g => g.Id, opt => opt.Ignore())
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

        //CreateMap<UpdateGroupDto, Group>()
        //.ForMember(g => g.CreationTime, opt => opt.Ignore())
        //.ForMember(g => g.CreatorUserId, opt => opt.Ignore());
    }
}