using AutoMapper;
using Webminux.Optician.Application.Invoices.dtos;

/// <summary>
/// This defines mapping for Invoice entity and its Dto.
/// </summary>
public class InvoiceMapProfile:Profile{
    
    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceMapProfile"/> class.
    /// </summary>
    public InvoiceMapProfile()
    {
        CreateMap<InvoiceDto, Invoice>();
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(i=>i.CustomerName,options=>options.MapFrom(invoice=>invoice.Customer.User.FullName));
    }
}