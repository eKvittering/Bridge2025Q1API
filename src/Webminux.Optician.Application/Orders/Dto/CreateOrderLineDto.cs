using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Orders.Dto
{
    /// <summary>
    /// Input Dto to create order lines
    /// </summary>
    public class CreateOrderLineDto
    {
        /// <summary>
        /// Date on which supplier has promissed to deliver order
        /// </summary>
        public virtual DateTime PromissedDate { get; set; }

        /// <summary>
        /// Quantity of product
        /// </summary>
        public virtual int Quantity { get; set; }

        /// <summary>
        /// Price of product
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// Product Id
        /// </summary>
        public virtual int ProductId { get; set; }

    }
}
