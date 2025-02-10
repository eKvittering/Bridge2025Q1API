using System;
using System.Collections.Generic;

namespace Webminux.Optician.BackgroundJobs.Dto
{

    public class BillyInvoiceDto
    {
        public string id { get; set; }
        public string organizationId { get; set; }
        public string type { get; set; }
        public DateTime createdTime { get; set; }
        public DateTime approvedTime { get; set; }
        public string contactId { get; set; }
        public object attContactPersonId { get; set; }
        public string entryDate { get; set; }
        public string paymentTermsMode { get; set; }
        public object paymentTermsDays { get; set; }
        public string dueDate { get; set; }
        public string state { get; set; }
        public string sentState { get; set; }
        public object externalId { get; set; }
        public object quoteId { get; set; }
        public string invoiceNo { get; set; }
        public string taxMode { get; set; }
        public decimal amount { get; set; }
        public decimal tax { get; set; }
        public decimal grossAmount { get; set; }
        public string currencyId { get; set; }
        public decimal exchangeRate { get; set; }
        public decimal balance { get; set; }
        public bool isPaid { get; set; }
        public object creditedInvoiceId { get; set; }
        public string contactMessage { get; set; }
        public string lineDescription { get; set; }
        public object recurringInvoiceId { get; set; }
        public string templateId { get; set; }
        public string downloadUrl { get; set; }
        public List<BillyInvoiceLineDto> lines { get; set; }
        public object orderNo { get; set; }
        public List<BillyPaymentMethodDto> paymentMethods { get; set; }
    }


}
