using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using Webminux.Optician.Products;

public class Group : CreationAuditedEntity, IMustHaveTenant
{
    public virtual int TenantId { get; set; }
    public virtual string Name { get; set; }

    public virtual ICollection<ProductResponsibleGroup> ResponsibleGroups { get; set; }

    public Group() { }

    public  Group Create(int tenantId, string name)
    {
        return new Group()
        {
            TenantId = tenantId,
            Name = name
        };
    }
}