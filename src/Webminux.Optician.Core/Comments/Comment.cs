using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Customers;

public class Comment : CreationAuditedEntity, IMustHaveTenant
{
    public virtual int TenantId { get; set; }
    public virtual string CommentText { get; set; }

    [ForeignKey(nameof(User))]
    public virtual long UserId { get; set; }
    public virtual User User { get; set; }

    [ForeignKey(nameof(Activity))]
    public virtual int ActivityId { get; set; }
    public virtual Activity Activity { get; set; }

    protected Comment() { }

    public static Comment Create(int tenantId, long userId, int activityId, string commentText)
    {
        return new Comment
        {
            TenantId = tenantId,
            UserId = userId,
            ActivityId = activityId,
            CommentText = commentText
        };
    }
}