using AutoMapper;

public class GroupMapProfile : Profile
{
    public GroupMapProfile()
    {
        CreateMap<GroupDto, Group>();
        CreateMap<Group, GroupDto>();
        CreateMap<CreateGroupDto, Group>()
        .ForMember(g => g.Id, opt => opt.Ignore())
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

        CreateMap<UpdateGroupDto, Group>()
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());
    }
}