using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Webminux.Optician.Authorization.Roles;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Configuration;
using Webminux.Optician.Localization;
using Webminux.Optician.MultiTenancy;
using Webminux.Optician.Timing;

namespace Webminux.Optician
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class OpticianCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            OpticianLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = OpticianConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
            
            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));
            Configuration.Localization.Languages.Add(new LanguageInfo("da", "Danish", "famfamfam-flags da"));
            Configuration.Localization.Languages.Add(new LanguageInfo("bg", "Bulgarian", "famfamfam-flags bg"));

            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = OpticianConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = OpticianConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(OpticianCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
