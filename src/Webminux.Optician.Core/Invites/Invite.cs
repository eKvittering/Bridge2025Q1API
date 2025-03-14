using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Customers;
using static Webminux.Optician.OpticianConsts;

public class Invite : CreationAuditedEntity, IMustHaveTenant
{
    public virtual int TenantId { get; set; }


    [ForeignKey(nameof(User))]
    public virtual long? UserId { get; set; }
    public virtual User User { get; set; }


    [ForeignKey(nameof(Group))]
    public virtual int? GroupId { get; set; }
    public virtual Group Group { get; set; }


    [ForeignKey(nameof(Activity))]
    public virtual int ActivityId { get; set; }
    public virtual Activity Activity { get; set; }

    public virtual InviteResponse Response { get; set; }

    protected Invite() { }

    public static Invite Create(int tenantId, int? userId, int? groupId, int activityId)
    {
        return new Invite
        {
            TenantId = tenantId,
            UserId = userId,
            GroupId = groupId,
            ActivityId = activityId,
            Response = InviteResponse.Pending
        };
    }
}