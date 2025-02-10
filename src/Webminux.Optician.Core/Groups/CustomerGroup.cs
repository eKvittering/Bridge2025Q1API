using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Core.Customers;

public class CustomerGroup : CreationAuditedEntity, IMustHaveTenant
{
    public virtual int TenantId { get; set; }

    [ForeignKey(nameof(Customer))]
    public virtual int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }

    [ForeignKey(nameof(Group))]
    public virtual int GroupId { get; set; }
    public virtual Group Group { get; set; }

    public CustomerGroup() { }

    public  CustomerGroup Create(int tenantId, int customerId, int groupId)
    {
        return new CustomerGroup
        {
            TenantId = tenantId,
            CustomerId = customerId,
            GroupId = groupId
        };
    }

}