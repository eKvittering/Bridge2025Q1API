using AutoMapper;
using Webminux.Optician.CustomFields;

namespace Webminux.Optician.MultiTenancy.Dto
{
    public class TenantMapProfile : Profile
    {
        public TenantMapProfile()
        {
            CreateMap<TenantDto, Tenant>()
                .ForMember(t => t.Company, options => options.Ignore());

            CreateMap<CustomFieldDto, CustomField>()
                .ForMember(c => c.Type, options => options.MapFrom(field => (OpticianConsts.CustomFieldType)field.Type))
                .ForMember(c => c.Screen, options => options.MapFrom(field => (OpticianConsts.Screen)field.Screen));
            CreateMap<CustomField, CustomFieldDto>()
                .ForMember(c => c.Type, options => options.MapFrom(field => (int)field.Type))
                .ForMember(c => c.Screen, options => options.MapFrom(field => (int)field.Screen));
        }
    }
}
