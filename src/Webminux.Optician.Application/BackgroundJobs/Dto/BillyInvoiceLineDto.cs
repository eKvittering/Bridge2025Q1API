namespace Webminux.Optician.BackgroundJobs.Dto
{
    public class BillyInvoiceLineDto
    {
        public string id { get; set; }
        public string invoiceId { get; set; }
        public BillyProductDto product { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public decimal? unitPrice { get; set; }
        public decimal? amount { get; set; }
        public decimal? tax { get; set; }
        public string taxRateId { get; set; }
        public object discountText { get; set; }
        public object discountMode { get; set; }
        public object discountValue { get; set; }
        public int priority { get; set; }
    }


}
