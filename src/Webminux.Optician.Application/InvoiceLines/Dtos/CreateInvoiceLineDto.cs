namespace Webminux.Optician.Application.InvoiceLines.Dtos
{
    /// <summary>
    /// This is a DTO class to create <see cref="InvoiceLine"/> entity.
    /// </summary>
    public class CreateInvoiceLineDto
    {
        
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

    }
}