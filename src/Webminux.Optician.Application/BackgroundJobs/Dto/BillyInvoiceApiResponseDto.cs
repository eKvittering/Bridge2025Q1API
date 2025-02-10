using System.Collections.Generic;

namespace Webminux.Optician.BackgroundJobs.Dto
{
    public class BillyInvoiceApiResponseDto
    {
        public BillyMetaDataDto meta { get; set; }
        public List<BillyInvoiceDto> invoices { get; set; }
    }
}
