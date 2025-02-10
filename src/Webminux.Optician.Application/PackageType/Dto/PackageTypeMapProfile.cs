using AutoMapper;
using Webminux.Optician.Categories;
using Webminux.Optician.PackageType;

public class PackageTypeMapProfile : Profile
{
    /// <summary>
    ///  maping PackageType
    /// </summary>
    public PackageTypeMapProfile()
    {
        CreateMap<PackageTypeDto, PackageType>();
        CreateMap<PackageType, PackageTypeDto>();
        CreateMap<CreateGroupDto, Group>()
        .ForMember(g => g.Id, opt => opt.Ignore())
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

    }
}