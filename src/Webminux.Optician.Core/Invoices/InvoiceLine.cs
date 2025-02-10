using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician.Faults;
using Webminux.Optician.Products;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Core.Invoices
{
    [Index(nameof(TenantId))]
    [Index(nameof(InvoiceId))]
    [Index(nameof(ProductNumber))]
    [Index(nameof(ProductSerialNoId))]
    public class InvoiceLine : CreationAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual string LineNo { get; set; }
        public virtual string Reference { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal CostPrice { get; set; }

        [ForeignKey(nameof(Invoice))]
        public virtual int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual string ProductNumber { get; set; }
        public virtual string ProductName { get; set; }
        public virtual double? Quantity { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string Status { get; set; }

        [ForeignKey(nameof(ProductSerialNo))]
        public int? ProductSerialNoId { get; set; }
        public Webminux.Optician.ProductItem.ProductItem ProductSerialNo { get; set; }
        public virtual ICollection<Fault> Faults { get; set; }

        protected InvoiceLine()
        {
            Faults = new HashSet<Fault>();
        }

        public static InvoiceLine Create(int tenantId, string lineNo, decimal amount, 
            decimal discount, decimal costPrice, int? invoiceId, string reference, 
            string serialNo, int? productSerialId, 
            string productNumber = "", string productName = "", double quantity = 0,string status=InvoiceLineStatuses.Draft)
        {
            return new InvoiceLine
            {
                TenantId = tenantId,
                LineNo = lineNo,
                Amount = amount,
                Discount = discount,
                CostPrice = costPrice,
                InvoiceId = invoiceId ?? 0,
                Reference = reference,
                ProductName = productName,
                ProductNumber = productNumber,
                Quantity = quantity,
                SerialNumber = serialNo,
                ProductSerialNoId = productSerialId,
                Status=status
            };
        }
    }
}