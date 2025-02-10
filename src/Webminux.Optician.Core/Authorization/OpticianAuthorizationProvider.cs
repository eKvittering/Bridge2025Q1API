using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Webminux.Optician.Authorization
{
    public class OpticianAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Customers, L("Customers"));
            context.CreatePermission(PermissionNames.Pages_Groups, L("Groups"));
            context.CreatePermission(PermissionNames.Pages_Activities, L("Activities"));
            context.CreatePermission(PermissionNames.Pages_Sales, L("Sales"));
            context.CreatePermission(PermissionNames.Pages_Import_Ecomonic_Data, L("ImportEconomicData"));
            context.CreatePermission(PermissionNames.Pages_Calender, L("Calender"));
            context.CreatePermission(PermissionNames.Pages_RememberReport, L("RememberReport"));
            context.CreatePermission(PermissionNames.Pages_CreateActivity, L("CreateActivity"));
            context.CreatePermission(PermissionNames.Pages_CreatePhoneCallNoteActivity, L("CreatePhoneCallNoteActivity"));
            context.CreatePermission(PermissionNames.Pages_CreateGroup, L("CreateGroup"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, OpticianConsts.LocalizationSourceName);
        }
    }
}
