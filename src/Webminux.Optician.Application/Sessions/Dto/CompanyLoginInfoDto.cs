using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Webminux.Optician.Companies;

namespace Webminux.Optician.Sessions.Dto
{
    [AutoMapFrom(typeof(Company))]
    public class CompanyLoginInfoDto:EntityDto
    {
        public virtual string Name { get; set; }
        public virtual string PrimaryColor { get; set; }
        public virtual string SecondaryColor { get; set; }
        public virtual string EconomicAgreementGrantToken { get; set; }
        public virtual string EconomicAppSecretToken { get; set; }
        public virtual string LogoUrl { get; set; }
    }
}
