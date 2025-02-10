using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Webminux.Optician.Orders.Dto
{
    /// <summary>
    /// DTO to Get and Update Order
    /// </summary>
    public class OrderDto:CreationAuditedEntityDto
    {
        /// <summary>
        /// Order No
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength), Required]
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// Date of Order creation
        /// </summary>
        public virtual DateTime OrderDate { get; set; }

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
        /// Name of employee who is responsible for order
        /// </summary>
        public virtual string EmployeeName { get; set; }

        /// <summary>
        /// Supplier who is responsible for order
        /// </summary>
        public virtual int SupplierId { get; set; }

        /// <summary>
        /// Name of supplier who is responsible for order
        /// </summary>
        public virtual string SupplierName { get; set; }

        /// <summary>
        /// List of Order Lines
        /// </summary>
        public virtual ICollection<OrderLineListDto> OrderLines { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrderDto()
        {
            OrderLines = new HashSet<OrderLineListDto>();
        }

    }
}
