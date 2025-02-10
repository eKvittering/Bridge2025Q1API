using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class EconomicCustomerDto
    {
        public int customerNumber { get; set; }
        public string currency { get; set; }
        public EconomicPaymentTermsDto paymentTerms { get; set; }
        public EconomicCustomerGroupDto customerGroup { get; set; }
        public string address { get; set; }
        public double balance { get; set; }
        public double dueAmount { get; set; }
        public string corporateIdentificationNumber { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string zip { get; set; }
        public EconomicVatZoneDto vatZone { get; set; }
        public DateTime lastUpdated { get; set; }
        public string contacts { get; set; }
        public EconomicTemplatesDto templates { get; set; }
        public EconomicTotalsDto totals { get; set; }
        public string deliveryLocations { get; set; }
        //public EconomicInvoicesDto invoices { get; set; }
        public bool eInvoicingDisabledByDefault { get; set; }
        public string self { get; set; }
        public double? creditLimit { get; set; }
        public string telephoneAndFaxNumber { get; set; }
        public string website { get; set; }
        public EconomicLayoutDto layout { get; set; }
        public bool? barred { get; set; }
        public string pNumber { get; set; }
        public string mobilePhone { get; set; }
    }
    public class EconomicLayoutDto
    {
        public int layoutNumber { get; set; }
        public string self { get; set; }
    }
    public class EconomicInvoicesDto
    {
        public string drafts { get; set; }
        public string booked { get; set; }
        public string self { get; set; }
    }

    public class EconomicTotalsDto
    {
        public string drafts { get; set; }
        public string booked { get; set; }
        public string self { get; set; }
    }

    public class EconomicTemplatesDto
    {
        public string invoice { get; set; }
        public string invoiceLine { get; set; }
        public string self { get; set; }
    }
    public class EconomicCustomerGroupDto
    {
        public int customerGroupNumber { get; set; }
        public string self { get; set; }
    }

    public class EconomicVatZoneDto
    {
        public int vatZoneNumber { get; set; }
        public string self { get; set; }
    }
    public class EconomicPaymentTermsDto
    {
        public int paymentTermsNumber { get; set; }
        public string self { get; set; }
    }


    public class EconomicProductGroupDto
    {
        public int productGroupNumber { get; set; }
        public string name { get; set; }

    }
    public class EconomicProductDto
    {
        public string productNumber { get; set; }
        public string name { get; set; }
        public double costPrice { get; set; }
        public double recommendedPrice { get; set; }
        public double salesPrice { get; set; }
        public string barCode { get; set; }
        public bool barred { get; set; }
        public string self { get; set; }
        public EconomicProductGroupDto productGroup { get; set; }
    }

    public class EconomicUnitDto
    {
        public int unitNumber { get; set; }
        public string name { get; set; }
        public string products { get; set; }
        public string self { get; set; }
    }

    public class EconomicInvoiceLineDto
    {
        public int lineNumber { get; set; }
        public int sortKey { get; set; }
        public string description { get; set; }
        public double quantity { get; set; }
        public double unitNetPrice { get; set; }
        public double discountPercentage { get; set; }
        public double unitCostPrice { get; set; }
        public double vatRate { get; set; }
        public double vatAmount { get; set; }
        public double totalNetAmount { get; set; }
        public EconomicProductDto product { get; set; }
        public EconomicUnitDto unit { get; set; }
    }

}
