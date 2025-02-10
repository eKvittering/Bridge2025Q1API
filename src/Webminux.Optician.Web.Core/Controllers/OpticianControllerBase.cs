using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Webminux.Optician.Controllers
{
    public abstract class OpticianControllerBase: AbpController
    {
        protected OpticianControllerBase()
        {
            LocalizationSourceName = OpticianConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
