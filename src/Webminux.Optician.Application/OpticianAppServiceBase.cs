using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.MultiTenancy;
using Abp.UI;

namespace Webminux.Optician
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class OpticianAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected OpticianAppServiceBase()
        {
            LocalizationSourceName = OpticianConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected virtual void ValidateTenant(){
            if(AbpSession.TenantId==null){
                throw new UserFriendlyException("You are not in a tenant!");
            }
        }
           protected virtual int GetTenantId(){
            return AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
        }
    }
}
