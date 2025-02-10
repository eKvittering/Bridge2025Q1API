using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using Webminux.Optician.Authorization.Users;

namespace Webminux.Optician.Faults.Dtos
{
    /// <summary>
    /// Data transfer object for Fault entity
    /// </summary>
    public class FaultDto : EntityDto, ICreationAudited
    {
        /// <summary>
        /// Information provided by client about fault
        /// </summary>
        public virtual string Comment { get; set; }


        public virtual string Description { get; set; }

        /// <summary>
        /// Email Provided by client 
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Date of fault.
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Base 64 string of image.
        /// </summary>
        public virtual List<FaultFile> Files{ get; set; }

        /// <summary>
        /// Id of responsible employee
        /// </summary>
        public virtual long? ResponsibleEmployeeId { get; set; }

        /// <summary>
        /// Name of responsible employee
        /// </summary>
        public virtual string ResponsibleEmployeeName { get; set; }

        /// <summary>
        /// Id of invoice
        /// </summary>
        public virtual int? InvoiceId { get; set; }

        /// <summary>
        /// No of invoice
        /// </summary>
        public virtual string InvoiceNo { get; set; }
        /// <summary>
        /// Id of invoice line.
        /// </summary>
        public virtual int? InvoiceLineId { get; set; }

        /// <summary>
        /// Name of Product
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// Status of fault
        /// </summary>
        public virtual string Status { get; set; }

        /// <summary>
        /// Id of activity created against fault
        /// </summary>
        public virtual int ActivityId { get; set; }

        /// <summary>
        /// id of product item 
        /// </summary>
        public virtual int? ProductItemId { get; set; }

        /// <summary>
        /// product serial number serial number 
        /// </summary>
        public virtual string ProductSerialNumber { get; set; }

        /// <summary>
        /// User Id of User who created fault
        /// </summary>
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// Creation time of fault
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Creation time of fault
        /// </summary>
        public virtual string SolutionNote { get; set; }

        /// <summary>
        /// Id of Supplier
        /// </summary>
        public virtual int? SupplierId { get; set; }
        
        /// <summary>
        /// Name of Supplier
        /// </summary>
        public virtual string SupplierName { get; set; }
        public virtual int? TicketId { get; set; }

        public virtual ActivityListDto ActivityDetail { get; set; }
        public virtual List<User> Employees { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual List<ActivityResponsible> ActivityResponsibles { get; set; }


    }
}
