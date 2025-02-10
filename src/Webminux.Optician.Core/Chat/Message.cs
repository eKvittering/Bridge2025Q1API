using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.MultiTenancy;
using System.Security.Permissions;

namespace Webminux.Optician.Chat
{
    public class Message : FullAuditedEntity, IMustHaveTenant
    {
        [ForeignKey(nameof(Tenant))]
        public virtual int TenantId { get; set; }
      //  public int Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }


    }
}
