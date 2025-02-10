using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.InvoiceLines.Dtos
{
    /// <summary>
    /// This is a DTO class for <see cref="InvoiceLine"/> entity.
    /// </summary>
    public class InvoiceLineDto : CreationAuditedEntityDto
    {
        /// <summary>
        /// Gets or sets Tenant Id.
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// Gets or sets LineNo.
        /// </summary>
        public virtual string LineNo { get; set; }
        public virtual string Reference { get; set; }

        /// <summary>
        /// Gets or sets Amount.
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets Discount.
        /// </summary>
        public virtual decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets CostPrice.
        /// </summary>
        public virtual decimal CostPrice { get; set; }

        /// <summary>
        /// Gets or sets ActivityId.
        /// </summary>
        public virtual int InvoiceId { get; set; }
        /// <summary>
        /// Gets or Sets ProductNumber
        /// </summary>
        public virtual string ProductNumber { get; set; }
        /// <summary>
        /// ProductName
        /// </summary>
        public virtual string ProductName { get; set; }
        /// <summary>
        ///  Quantity
        /// </summary>
        public virtual double? Quantity { get; set; }

        /// <summary>
        /// Product item serial number
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Product Item Id
        /// </summary>
        public int? ProductSerialId { get; set; }

        /// <summary>
        /// Status of Invoice Line
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Product Id for mapping purpose on client side
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// Supplier Id
        /// </summary>
        public int? SupplierId { get; set; }

        /// <summary>
        /// Name of supplier
        /// </summary>
        public string SupplierName { get; set; }
    }
}