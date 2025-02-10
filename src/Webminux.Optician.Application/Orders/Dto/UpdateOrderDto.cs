using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Orders.Dto
{
    /// <summary>
    /// Data transfer object to edit Order
    /// </summary>
    public class UpdateOrderDto:EntityDto
    {
        /// <summary>
        /// Order No
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength), Required]
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// Date of Order creation
        /// </summary>
        public virtual string OrderDate { get; set; }

        /// <summary>
        /// Additional information about order
        /// </summary>
        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Note { get; set; }

        /// <summary>
        /// Indicates if order is received
        /// </summary>
        public virtual bool Received { get; set; }

        /// <summary>
        /// Employee who is responsible for order
        /// </summary>
        public virtual long EmployeeId { get; set; }

        /// <summary>
        /// Supplier who is responsible for order
        /// </summary>
        public virtual int SupplierId { get; set; }

        /// <summary>
        /// List of Order Lines
        /// </summary>
        public virtual ICollection<OrderLineDto> OrderLines { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public UpdateOrderDto()
        {
            OrderLines = new HashSet<OrderLineDto>();
        }

    }
}
