using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Invoices.dtos
{
    public class SaveDraftInvoiceForAdminDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SubCustomerId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int InvoiceId { get; set; }
    }
}
