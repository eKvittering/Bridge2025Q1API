using AutoMapper;
using Webminux.Optician.Categories;
using Webminux.Optician.Package;

public class PackageMapProfile : Profile
{
    /// <summary>
    ///  maping Package
    /// </summary>
    public PackageMapProfile()
    {
        CreateMap<PackageDto, Webminux.Optician.Package.Pacakge>();
        CreateMap<Webminux.Optician.Package.Pacakge, PackageDto>();
        CreateMap<CreateGroupDto, Group>()
        .ForMember(g => g.Id, opt => opt.Ignore())
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

    }
}