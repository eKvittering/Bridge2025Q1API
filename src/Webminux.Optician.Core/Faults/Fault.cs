using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Invoices;
using Webminux.Optician.Suppliers;
using Webminux.Optician.Tickets;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Faults
{
    public class Fault : CreationAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        /// <summary>
        /// Information provided by client about fault
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Comment { get; set; }

        public virtual string Description { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string SolutionNote { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Email { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual FaultStatus Status { get; set; }

        [ForeignKey(nameof(ResponsibleEmployee))]
        public long? ResponsibleEmployeeId { get; set; }
        public User ResponsibleEmployee { get; set; }

        [ForeignKey(nameof(InvoiceLine))]
        public virtual int? InvoiceLineId { get; set; }
        public virtual InvoiceLine InvoiceLine { get; set; }

        [ForeignKey(nameof(Activity))]
        public virtual int? ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        [ForeignKey(nameof(ProductItem))]
        public virtual int? ProductItemId { get; set; }
        public virtual Webminux.Optician.ProductItem.ProductItem ProductItem { get; set; }

        [ForeignKey(nameof(Supplier))]
        public virtual int? SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<FaultFile> Files { get; set; }

        [ForeignKey(nameof(Ticket))]
        public virtual int? TicketId { get; set; }
        public virtual Ticket ticket { get; set; }

        protected Fault() { }

        public static Fault Create(int tenantId, int activityId, string comment, string description, long? responsibleEmployeeId, string email, DateTime date, int? invoiceLineId, int? productItemId, int? supplierId, int? ticketId)
        {
            return new Fault
            {
                TenantId = tenantId,
                Date = date,
                Email = email,
                ResponsibleEmployeeId = responsibleEmployeeId,
                Comment = comment,
                Description = description,
                Status = FaultStatus.Open,
                InvoiceLineId = invoiceLineId,
                ActivityId = activityId,
                ProductItemId = productItemId,
                SolutionNote = string.Empty,
                SupplierId = supplierId,
                TicketId = ticketId
            };
        }

    }
}
