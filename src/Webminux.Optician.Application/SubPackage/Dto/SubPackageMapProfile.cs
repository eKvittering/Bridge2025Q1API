using AutoMapper;

public class SubPackageMapProfile : Profile
{
    
    public SubPackageMapProfile()
    {
        CreateMap<SubPackageDto, Webminux.Optician.SubPackage.SubPackage>();
        CreateMap<Webminux.Optician.SubPackage.SubPackage, SubPackageDto>();
        CreateMap<CreateGroupDto, Group>()
        .ForMember(g => g.Id, opt => opt.Ignore())
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

    }
}