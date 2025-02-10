using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Authorization.Users;

public class EmployeeGroup : CreationAuditedEntity, IMustHaveTenant
{
    public virtual int TenantId { get; set; }

    [ForeignKey(nameof(User))]
    public virtual long? EmployeeId { get; set; }
    public virtual User User { get; set; }

    [ForeignKey(nameof(Group))]
    public virtual int GroupId { get; set; }
    public virtual Group Group { get; set; }

    public EmployeeGroup() { }

    public EmployeeGroup Create(int tenantId, int UserId, int groupId)
    {
        return new EmployeeGroup
        {
            TenantId = tenantId,
            EmployeeId = UserId,
            GroupId = groupId
        };
    }

}