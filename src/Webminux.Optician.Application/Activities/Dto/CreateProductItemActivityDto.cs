using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Activities.Dto
{
    public class CreateProductItemActivityDto : CreateProductItemDto
    {

        /// <summary>
        /// Customer Id
        /// </summary>
        public virtual long CustomerId { get; set; }
        
        /// <summary>
        /// Product Item Id
        /// </summary>
        public virtual int ProductItemId { get; set; }

        /// <summary>
        /// Contains note from client
        /// </summary>
        public string Note { get; set; }
    }
}
