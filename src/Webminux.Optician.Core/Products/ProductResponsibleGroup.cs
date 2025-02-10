using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Tickets;

namespace Webminux.Optician.Products
{
    public class ProductResponsibleGroup : FullAuditedEntity
    {
        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public ProductResponsibleGroup() { }

        public static ProductResponsibleGroup Create(int productId, int groupId)
        {
            return new ProductResponsibleGroup
            {
                ProductId = productId,
                GroupId = groupId,
            };
        }
    }
}
