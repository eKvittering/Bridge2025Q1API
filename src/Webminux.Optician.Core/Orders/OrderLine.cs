using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Orders
{
    public class OrderLine : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual DateTime PromissedDate { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Price { get; set; }

        [ForeignKey(nameof(Order))]
        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey(nameof(Product))]
        public virtual int ProductId { get; set; }
        public virtual Product Product { get; set; }

        protected OrderLine()
        {
        }

        public static OrderLine Create(int tenantId, DateTime promissedDate, int quantity, decimal price, int productId)
        {
            var orderLine = new OrderLine
            {
                TenantId = tenantId,
                PromissedDate = promissedDate,
                Quantity = quantity,
                Price = price,
                ProductId = productId
            };

            return orderLine;
        }
    }
}
