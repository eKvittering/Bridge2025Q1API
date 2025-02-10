using System.Collections.Generic;

namespace Webminux.Optician.BackgroundJobs.Dto
{
    public class BillyProductDto
    {
        public string id { get; set; }
        public string organizationId { get; set; }
        public string name { get; set; }
        public string externalId { get; set; }
        public string description { get; set; }
        public string accountId { get; set; }
        public string inventoryAccountId { get; set; }
        public string productNo { get; set; }
        public string suppliersProductNo { get; set; }
        public string salesTaxRulesetId { get; set; }
        public bool isArchived { get; set; }
        public bool isInInventory { get; set; }
        public string imageId { get; set; }
        public string imageUrl { get; set; }
        public List<BillyProductPriceDto> prices { get; set; }
    }
    public class BillyProductPriceDto
    {
        public string id { get; set; }
        public string productId { get; set; }
        public int unitPrice { get; set; }
        public string currencyId { get; set; }
    }

}
