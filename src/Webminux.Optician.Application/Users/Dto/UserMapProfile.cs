using AutoMapper;
using Webminux.Optician.Application;
using Webminux.Optician.Authorization.Users;

namespace Webminux.Optician.Users.Dto
{
    /// <summary>
    /// Define Auto Mapper Profiles for Users.
    /// </summary>
    public class UserMapProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapProfile"/> class.
        /// </summary>
        public UserMapProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<UserDto, User>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateUserDto, User>();
            CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());
            CreateMap<User,UserListDto>().ForMember(x=>x.UserTypeName,opt=>opt.MapFrom(x=>x.UserType.Name));
        }
    }
}
