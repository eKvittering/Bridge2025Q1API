using Abp.AutoMapper;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Webminux.Optician.Authorization;

namespace Webminux.Optician
{
    [DependsOn(
        typeof(OpticianCoreModule), 
        typeof(AbpAutoMapperModule),
        typeof(AbpHangfireAspNetCoreModule))
        ]
    public class OpticianApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<OpticianAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(OpticianApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);
            Configuration.BackgroundJobs.UseHangfire();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
