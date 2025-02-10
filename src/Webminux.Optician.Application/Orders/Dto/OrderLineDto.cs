using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Orders.Dto
{
    /// <summary>
    /// Data Transfer object to create and edit order line
    /// </summary>
    public class OrderLineDto : EntityDto
    {
        /// <summary>
        /// Date on which supplier has promissed to deliver order
        /// </summary>
        public virtual string PromissedDate { get; set; }

        /// <summary>
        /// Quantity of product
        /// </summary>
        public virtual int Quantity { get; set; }

        /// <summary>
        /// Price of product
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// Order Id
        /// </summary>
        public virtual int OrderId { get; set; }
        /// <summary>
        /// Product Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// Provide Total Value of Order Line
        /// </summary>
        public virtual decimal Total { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrderLineDto()
        {
            Total = Quantity * Price;
        }
    }
}
