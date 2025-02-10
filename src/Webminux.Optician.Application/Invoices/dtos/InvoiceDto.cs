using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Application.InvoiceLines.Dtos;

namespace Webminux.Optician.Application.Invoices.dtos
{
    /// <summary>
    /// This is a DTO class for <see cref="Invoice"/> entity.
    /// </summary>
    public class InvoiceDto : EntityDto, ICreationAudited
    {

        /// <summary>
        /// Gets or sets the InvoiceNo.
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string InvoiceNo { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceDate.
        /// </summary>
        public virtual DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Gets or sets the DueDate.
        /// </summary>
        public virtual DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the Currency.
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Currency { get; set; }

        /// <summary>
        /// Gets or sets the Amount.
        /// </summary>
        public virtual decimal Amount { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal RemainAmount { get; set; }

        /// <summary>
        /// Gets or sets the Comment.
        /// </summary>
        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Comment { get; set; }

        /// <summary>
        /// Gets or sets the CustomerId.
        /// </summary>
        public virtual int? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the Customer User Id.
        /// </summary>
        public virtual long? CustomerUserId { get; set; }

        /// <summary>
        /// Gets or sets the Customer Name.
        /// </summary>
        public virtual string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the Sub Customer Name.
        /// </summary>
        public virtual string SubCustomerName { get; set; }

        /// <summary>
        /// Gets or sets the CreatorUserId.
        /// </summary>
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// Gets or sets the CreationTime.
        /// </summary>
        /// 
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the SerialNumber.
        /// </summary>
        ///
        public virtual string SerialNumber { get; set; }

        /// <summary>
        /// List of invoice lines against that order
        /// </summary>
        public virtual ICollection<InvoiceLineDto> InvoiceLines { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InvoiceDto()
        {
            InvoiceLines = new HashSet<InvoiceLineDto>();
        }
    }
}