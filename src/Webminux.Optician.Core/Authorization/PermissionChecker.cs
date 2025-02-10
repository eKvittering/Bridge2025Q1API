using Abp.Authorization;
using Webminux.Optician.Authorization.Roles;
using Webminux.Optician.Authorization.Users;

namespace Webminux.Optician.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
