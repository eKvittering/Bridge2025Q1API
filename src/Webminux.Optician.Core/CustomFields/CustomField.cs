using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.CustomFields
{
    public class CustomField : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        [Required]
        [StringLength(MaxTitleLength)]
        public virtual string Label { get; set; }
        public virtual CustomFieldType Type { get; set; }

        public virtual Screen Screen { get; set; }

        protected CustomField() { }

        public static CustomField Create(int tenantId, string label, CustomFieldType type, Screen screen)
        {
            return new CustomField
            {
                TenantId = tenantId,
                Label = label,
                Type = type,
                Screen = screen
            };
        }
    }
}
