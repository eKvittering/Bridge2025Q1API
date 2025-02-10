using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.InvoiceLines.Dtos
{
    /// <summary>
    /// This is a DTO class use as input to GetAllAsync method.
    /// </summary>
    public class GetPagedInvoiceLinesResultRequestDto:PagedResultRequestDto
    {
        /// <summary>
        /// Gets or sets InvoiceId.
        /// </summary>
        public virtual int? InvoiceId { get; set; }
    }
}