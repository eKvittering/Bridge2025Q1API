using AutoMapper;
using Webminux.Optician.Core.Invoices;

namespace Webminux.Optician.Application.InvoiceLines.Dtos
{
    /// <summary>
    /// This is a class for <see cref="InvoiceLine"/> entity Mapping Profiles.
    /// </summary>
    public class InvoiceLineMapProfile:Profile
    {
        public InvoiceLineMapProfile()
        {
            CreateMap<InvoiceLine, InvoiceLineDto>();
            CreateMap<InvoiceLineDto, InvoiceLine>();
        }
    }
}