using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    /// <summary>
    /// DTO to transfer CustomFieldsInformation
    /// </summary>
    public class EntityFieldMappingDto
    {
        /// <summary>
        /// Id of custom field
        /// </summary>
        public virtual int CustomFieldId { get; set; }

        /// <summary>
        /// Id of Entity Object
        /// </summary>
        public virtual long? ObjectId { get; set; }

        /// <summary>
        /// Value of custom field
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// Label of Custom Field
        /// </summary>
        public virtual string Label { get; set; }

        /// <summary>
        /// Type of custom field.
        /// </summary>
        public virtual int Type { get; set; }

        /// <summary>
        /// Screen on which custom field display
        /// </summary>
        public virtual int Screen { get; set; }

    }
}
