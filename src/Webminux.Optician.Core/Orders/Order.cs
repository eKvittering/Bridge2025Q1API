using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Suppliers;

namespace Webminux.Optician.Orders
{
    public class Order : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength), Required]
        public virtual string OrderNumber { get; set; }
        public virtual DateTime OrderDate { get; set; }

        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Note { get; set; }
        public virtual bool Received { get; set; }

        [ForeignKey(nameof(Employee))]
        public virtual long EmployeeId { get; set; }
        public virtual User Employee { get; set; }

        [ForeignKey(nameof(Supplier))]
        public virtual int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }

        protected Order()
        {
            OrderLines = new HashSet<OrderLine>();
        }

        public static Order Create(int tenantId, string orderNumber, DateTime orderDate, string note, bool received, long employeeId, int supplierId, ICollection<OrderLine> orderLines)
        {
            var order = new Order
            {
                TenantId = tenantId,
                OrderNumber = orderNumber,
                OrderDate = orderDate,
                Note = note,
                Received = received,
                EmployeeId = employeeId,
                SupplierId = supplierId,
                OrderLines = orderLines
            };

            return order;
        }
    }
}
