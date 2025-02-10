using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.MultiTenancy.Dto
{
    /// <summary>
    /// DTO to transfer custom fields data.
    /// </summary>
    public class CustomFieldDto:EntityDto
    {
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
