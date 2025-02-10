using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Webminux.Optician.Orders.Dto
{
    /// <summary>
    /// DTO to Get and Update Order Line
    /// </summary>
    public class OrderLineListDto : EntityDto
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
        /// Total amount of order line
        /// </summary>        
        public virtual decimal Total { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public OrderLineListDto()
        {
             Total=Quantity*Price;
        }

    }
}
