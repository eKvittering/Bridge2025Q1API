using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Webminux.Optician
{
    public class ProductGroup : FullAuditedEntity, IMustHaveTenant, ILookupDto<int>
    {
        public int ProductGroupNumber { get; set; }
        public string Name { get; set; }
        public string Domestic { get; set; }
        public string EU { get; set; }
        public string Abroad { get; set; }
        public int TenantId { get; set; }

        public ProductGroup()
        {

        }

        public static ProductGroup Create(int productGroupNumber, string name, string domestic, string eu, string abroad, int tenantId)
        {
            return new ProductGroup
            {
                ProductGroupNumber = productGroupNumber,
                Abroad = abroad,
                EU = eu,
                Domestic = domestic,
                Name = name,
                TenantId = tenantId,
               
                
            };
        } 
    
    }
}
