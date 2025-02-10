using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.InvoiceLines.Dtos
{
    /// <summary>
    /// Dto to add serial no against invoiceline
    /// </summary>
    public class AddSerialNumberDto : EntityDto
    {
        /// <summary>
        /// Product Item Id
        /// </summary>
        public int ProductItemId { get; set; }
        public string SerialNumber { get; set; }
    }
}
