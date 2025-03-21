﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization.Users;
using Abp.Domain.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Abp.UI;
using Webminux.Optician.Authorization.Roles;
using Webminux.Optician.MultiTenancy;

namespace Webminux.Optician.Authorization.Users
{
    public class UserRegistrationManager : DomainService
    {
        public IAbpSession AbpSession { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;

            AbpSession = NullAbpSession.Instance;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed , int? userTypeId)
        {
            //CheckForTenant();

            var tenant = await GetActiveTenantAsync();

            var user = new User
            {
                TenantId = tenant != null ? tenant.Id : null,
                Name = name,
                UserTypeId = (int)userTypeId,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = true,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();
            if(tenant != null)
            {
                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
                }               
            }
            int? tenatId = tenant != null ? tenant.Id : null;
            await _userManager.InitializeOptionsAsync(tenatId);


            try
            {
                var result = await _userManager.CreateAsync(user, plainPassword);
                CheckErrors(result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"User registration failed: {ex.Message}", ex);
            }
            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {       
            // set default tenant id if not exists
            var tenantId = AbpSession.TenantId ?? 1;

            return await GetActiveTenantAsync(tenantId);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
