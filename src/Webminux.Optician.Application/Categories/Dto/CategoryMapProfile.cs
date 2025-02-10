using AutoMapper;
using Webminux.Optician.Categories;

public class CategoryMapProfile : Profile
{
    /// <summary>
    ///  maping category
    /// </summary>
    public CategoryMapProfile()
    {
        CreateMap<CategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateGroupDto, Group>()
        .ForMember(g => g.Id, opt => opt.Ignore())
        .ForMember(g => g.CreationTime, opt => opt.Ignore())
        .ForMember(g => g.CreatorUserId, opt => opt.Ignore());

        //CreateMap<UpdateGroupDto, Group>()
        //.ForMember(g => g.CreationTime, opt => opt.Ignore())
        //.ForMember(g => g.CreatorUserId, opt => opt.Ignore());
    }
}