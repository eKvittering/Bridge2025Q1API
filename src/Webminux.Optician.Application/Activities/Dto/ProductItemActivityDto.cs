using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Activities.Dto
{
    public class ProductItemActivityDto
    {
        public int PreviousProductItemId { get; set; }
        public int ProductItemId { get; set; }
        public int? ActivityId { get; set; }
        public int? InvoiceId { get; set; }
        public int? InvoiceLineId { get; set; }
        public string SerialNumber { get; set; }
        public string ProductName { get; set; }
        public string Note { get; set; }
    }
}
