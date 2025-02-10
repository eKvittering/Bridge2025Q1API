using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Invoices.dtos
{
    /// <summary>
    /// Input data for creating draft invoice
    /// </summary>
    public class AddItemToDraftInvoiceDto
    {
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
        /// Id of customer
        /// </summary>
        public int? CustomerId { get; set; }


        /// <summary>
        /// Id of Admin
        /// </summary>
        public long? AdminId { get; set; }  
    }
}
