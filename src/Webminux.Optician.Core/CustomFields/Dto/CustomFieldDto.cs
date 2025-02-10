using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.CustomFields.Dto
{
    public class CustomFieldDto
    {
        /// <summary>
        /// Custom Fields for customer entity
        /// </summary>
        public virtual ICollection<EntityFieldMappingDto> CustomFields { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomFieldDto()
        {
            CustomFields = new HashSet<EntityFieldMappingDto>();
        }
    }
}
