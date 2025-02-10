using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Application.InvoiceLines.Dtos;

namespace Webminux.Optician.Faults.Dtos
{
    /// <summary>
    /// Data transfer object to create fault
    /// </summary>
    public class CreateFaultDto
    {
        /// <summary>
        /// Information provided by client about fault
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Information provided by client about fault
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Email Provided by client 
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Date of fault.
        /// </summary>
        public virtual string Date { get; set; }

        /// <summary>
        /// Base 64 string of image.
        /// </summary>
        public virtual ICollection<CreateFaultFileDto> Files { get; set; }

        /// <summary>
        /// Id of invoice line.
        /// </summary>
        public virtual int InvoiceLineId { get; set; }

        /// <summary>
        /// Id of Product Item.
        /// </summary>
        public virtual int? ProductItemId { get; set; }

        /// <summary>
        /// Id of Supplier
        /// </summary>
        public virtual int? SupplierId { get; set; }

        public virtual int? TicketId { get; set; }

        public virtual List<long> EmployeeIds { get; set; }

        public virtual List<int> GroupIds { get; set; }
    }
}
