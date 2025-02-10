using AutoMapper;
using Webminux.Optician.BackgroundJobs.Dto;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Customers.Dtos;

namespace Webminux.Optician.Application.Customers.Dtos
{
    /// <summary>
    /// This defines mapping for Customer entity and its Dto.
    /// </summary>
    public class CustomerMapProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerMapProfile"/> class.
        /// </summary>
        public CustomerMapProfile()
        {
            CreateMap<CustomerDto, Customer>();
            CreateMap<EconomicCustomerDto, CreateCustomerDto>()
                .ForMember(x => x.UserName, m => m.MapFrom(s => s.email))
                .ForMember(x => x.EmailAddress, m => m.MapFrom(s => s.email))
                .ForMember(x => x.Name, m => m.MapFrom(s => s.name))
                .ForMember(x => x.TelephoneFax, m => m.MapFrom(s => s.telephoneAndFaxNumber))
                .ForMember(x => x.TownCity, m => m.MapFrom(s => s.city))
                .ForMember(x => x.Postcode, m => m.MapFrom(s => s.zip))
                .ForMember(x => x.CustomerNo, m => m.MapFrom(s => s.customerNumber.ToString()));
            CreateMap<BillyContactDto, CreateCustomerDto>()
                .ForMember(x => x.UserName, m => m.MapFrom(s => s.id + "@billy.com"))
                .ForMember(x => x.EmailAddress, m => m.MapFrom(s => s.id + "@billy.com"))
                .ForMember(x => x.Name, m => m.MapFrom(s => s.name))
                .ForMember(x => x.TelephoneFax, m => m.MapFrom(s => s.contactNo))
                .ForMember(x => x.TownCity, m => m.MapFrom(s => s.cityText))
                .ForMember(x => x.Postcode, m => m.MapFrom(s => s.zipcodeText))
                .ForMember(x => x.CustomerNo, m => m.MapFrom(s => s.id));
        }
    }
}