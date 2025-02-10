using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Core.SubCustomers;

namespace Webminux.Optician.SubCustomers.Dtos
{
    public class SubCustomerMapProfile:Profile
    {
        public SubCustomerMapProfile()
        {
            CreateMap<SubCustomer,SubCustomerDto>();
            CreateMap<SubCustomerDto, SubCustomer>();
        }
    }
}
