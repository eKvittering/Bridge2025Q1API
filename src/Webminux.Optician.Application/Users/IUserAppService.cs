using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application;
using Webminux.Optician.Application.Users.Dto;
using Webminux.Optician.Roles.Dto;
using Webminux.Optician.Users.Dto;

namespace Webminux.Optician.Users
{
    /// <summary>
    /// An interface for the user application service.
    /// </summary>
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        /// <summary>
        /// Gets All users of required type.
        /// </summary>
       Task<ListResultDto<UserDto>> GetFilteredUsersAsync(GetAllInputDto input);

        /// <summary>
        /// Deactivate user.
        /// </summary>
        Task DeActivateAync(EntityDto<long> user);

        /// <summary>
        /// Activate user.
        /// </summary>
        Task Activate(EntityDto<long> user);

        /// <summary>
        /// Get all roles.
        /// </summary>
        Task<ListResultDto<RoleDto>> GetRolesAsync();

        /// <summary>
        /// Change Language of application.
        /// </summary>
        Task ChangeLanguageAsync(ChangeUserLanguageDto input);

        /// <summary>
        /// Change Password of user.
        /// </summary>
        Task<bool> ChangePassword(ChangePasswordDto input);

        /// <summary>
        /// Get all users with pagination.
        /// </summary>
        Task<PagedResultDto<UserListDto>> GetPagedResultAsync(PagedUserResultRequestDto input);


        /// <summary>
        /// Gets All users .
        /// </summary>
        Task<ListResultDto<UserDto>> GetAllUsers();

        /// <summary>
        ///  get by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDto> GetByUserId(long userId);
    

    }
}
