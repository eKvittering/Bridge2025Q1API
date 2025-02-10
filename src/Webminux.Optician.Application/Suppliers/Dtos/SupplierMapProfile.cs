using AutoMapper;
using Webminux.Optician.BackgroundJobs.Dto;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Suppliers;

namespace Webminux.Optician.Application.Suppliers.Dtos
{
    /// <summary>
    /// This defines mapping for Customer entity and its Dto.
    /// </summary>
    public class SupplierMapProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierMapProfile"/> class.
        /// </summary>
        public SupplierMapProfile()
        {
            CreateMap<SupplierDto, Supplier>();
        }
    }
}