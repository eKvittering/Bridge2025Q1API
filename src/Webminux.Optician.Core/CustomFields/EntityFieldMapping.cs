using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.CustomFields
{
    public class EntityFieldMapping : Entity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Value { get; set; }

        public virtual long ObjectId { get; set; }

        [ForeignKey(nameof(CustomField))]
        public virtual int CustomFieldId { get; set; }
        public virtual CustomField CustomField { get; set; }

        protected EntityFieldMapping() { }

        public static EntityFieldMapping Create(int tenantId,string value,long objectId,int customFieldId)
        {
            return new EntityFieldMapping
            {
                TenantId = tenantId,
                Value = value,
                ObjectId = objectId,
                CustomFieldId = customFieldId
            };
        }
    }
}
