using AutoMapper;
using Webminux.Optician.Sites;

public class SiteMapProfile : Profile
{
    public SiteMapProfile()
    {
        CreateMap<SiteDto, Site>();
        CreateMap<Site, SiteDto>();
        CreateMap<CreateSiteDto, Site>()
        .ForMember(g => g.Id, opt => opt.Ignore())
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

        CreateMap<UpdateSiteDto, Site>()
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());
    }
}