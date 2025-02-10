using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webminux.Optician.Rooms
{
    public class Room : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Name { get; set; }

        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Descriptions { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        protected Room() { }
        public static Room Create(int tenantId, string name, string description)
        {
            var room = new Room
            {
                TenantId = tenantId,
                Name = name,
                Descriptions = description,
            };

            return room;
        }
    }
}
