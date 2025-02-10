using Webminux.Optician.Users.Dto;

namespace Webminux.Optician.Application
{
    /// <summary>
    /// Data transfer object for Users listing.
    /// </summary>
    public class UserListDto : UserDto
    {

        /// <summary>
        /// Name of User Type.
        /// </summary>
        public string UserTypeName { get; set; }
    }
}