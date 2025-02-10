using Abp.AutoMapper;
using Webminux.Optician.Authentication.External;

namespace Webminux.Optician.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
