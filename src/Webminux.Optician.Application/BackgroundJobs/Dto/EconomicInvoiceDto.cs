using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class EconomicRecipientDto
    {
        public string name { get; set; }
        public string address { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public EconomicVatZoneDto vatZone { get; set; }
    }

    public class EconomicPdfDto
    {
        public string download { get; set; }
    }

    public class EconomicInvoiceDto
    {
        public int bookedInvoiceNumber { get; set; }
        public DateTime date { get; set; }
        public string currency { get; set; }
        public double exchangeRate { get; set; }
        public double netAmount { get; set; }
        public double netAmountInBaseCurrency { get; set; }
        public double grossAmount { get; set; }
        public double grossAmountInBaseCurrency { get; set; }
        public double vatAmount { get; set; }
        public double roundingAmount { get; set; }
        public double remainder { get; set; }
        public double remainderInBaseCurrency { get; set; }
        public DateTime dueDate { get; set; }
        public EconomicPaymentTermsDto paymentTerms { get; set; }
        public EconomicCustomerDto customer { get; set; }
        public EconomicRecipientDto recipient { get; set; }
        public EconomicLayoutDto layout { get; set; }
        public EconomicPdfDto pdf { get; set; }
        public string sent { get; set; }
        public string self { get; set; }
        public List<EconomicInvoiceLineDto> lines { get; set; }
    }


}
