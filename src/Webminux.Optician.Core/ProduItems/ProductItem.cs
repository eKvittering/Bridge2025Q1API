using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Invoices;

namespace Webminux.Optician.ProductItem
{
    public class ProductItem : FullAuditedEntity, IMustHaveTenant, ILookupDto<int>
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public bool IsSold { get; set; } = false;
        public bool IsAvailable { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(ReceiverEmployee))]
        public long RecieverEmployeeId { get; set; }
        public User ReceiverEmployee { get; set; }

        public DateTime ReceiverEmployeeDate { get; set; }

        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public string Note { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey(nameof(Activity))]
        public int? ActivityId { get; set; }
        public Activity Activity { get; set; }

        [ForeignKey(nameof(Invoice))]
        public int? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        [ForeignKey(nameof(InvoiceLine))]
        public int? InvoiceLineId { get; set; }
        public InvoiceLine InvoiceLine { get; set; }
        protected ProductItem() { }

        public static ProductItem Create(int tenantId, string serialNumber, int productId, long receiverId, int? activityId, int? invoiceId, int? invoiceLineId, string code,string description)
        {
            return new ProductItem
            {
                TenantId = tenantId,
                SerialNumber = serialNumber,
                ProductId = productId,
                RecieverEmployeeId = receiverId,
                ReceiverEmployeeDate = DateTime.Now,
                Name = serialNumber,
                ActivityId = activityId,
                InvoiceId = invoiceId,
                InvoiceLineId = invoiceLineId,
                IsAvailable = true,
                Code = string.IsNullOrWhiteSpace(code) ? OpticianConsts.DefaultItemCode : code,
                Description = description
            };
        }
    }
}