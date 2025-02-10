using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MenuItemDto : EntityDto, ICreationAudited
{
    /// <summary>
    /// set Brand name
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// set parent id if child defaul  = 0
    /// </summary>
    public int TenantId { get; set; }


    public int OrderNo { get; set; }

    /// <summary>
    /// default activi Brand 
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// default activi Brand 
    /// </summary>
    public string RouterLink{ get; set; }

    /// <summary>
    /// Gets or sets Creator User Id.
    /// </summary>
    public long? CreatorUserId { get; set; }

    /// <summary>
    /// Gets or sets Creation Time.
    /// </summary>
    public DateTime CreationTime { get; set; }
}
