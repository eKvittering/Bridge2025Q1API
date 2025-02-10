using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Webminux.Optician.Configuration;

namespace Webminux.Optician.Web.Host.Startup
{
    [DependsOn(
       typeof(OpticianWebCoreModule))]
    public class OpticianWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public OpticianWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(OpticianWebHostModule).GetAssembly());
        }
    }
}
