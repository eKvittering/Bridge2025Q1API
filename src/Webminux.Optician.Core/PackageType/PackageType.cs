using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Chat;
using Webminux.Optician.Faults;
using Webminux.Optician.MultiTenancy;
using static Webminux.Optician.Authorization.Roles.StaticRoleNames;

namespace Webminux.Optician.PackageType
{
    public class PackageType : FullAuditedEntity,IMustHaveTenant
    {
        [ForeignKey(nameof(Tenant))]
        public virtual int TenantId { get; set; }
        public string Name { get; set; }
        public int senderTypeId {  get; set; }
      //  [ForeignKey(nameof(ActivityType))]
        public virtual int FollowUpTypeId { get; set; }
      //  public virtual ActivityType Activity { get; set; }

        [ForeignKey(nameof(User))]
        public long? UserId { get; set; }
        public User User { get; set; }

        public virtual int FaultId { get; set; }
     


        public static PackageType Create(int tenantId, string name,int followUpTypeId,int senderTypeId,long? UserId, int FaultId)
        {
            var model = new PackageType
            {
                Name = name,
                FollowUpTypeId = followUpTypeId,
                FaultId = FaultId,
                UserId = UserId,
                senderTypeId = senderTypeId,
                TenantId = tenantId,
                CreatorUserId = UserId,
                CreationTime = DateTime.UtcNow,
                LastModificationTime = DateTime.UtcNow,
                

            };

            return model;
        }


    }
}
