using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Webminux.Optician.Application.InvoiceLines.Dtos;

namespace Webminux.Optician.Application.Invoices.dtos
{
    /// <summary>
    /// This is a DTO class to create <see cref="Invoice"/> entity.
    /// </summary>
    public class CreateInvoiceDto
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

        /// <summary>
        /// Gets or sets the Comment.
        /// </summary>
        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Comment { get; set; }

        /// <summary>
        /// Gets or sets the CustomerId.
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// List of invoice lines against that order
        /// </summary>
        public virtual ICollection<InvoiceLineDto> InvoiceLines { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CreateInvoiceDto()
        {
            InvoiceLines = new HashSet<InvoiceLineDto>();
        }
    }
}