using System.Collections.Generic;
using Abp.Application.Services.Dto;

/// <summary>
/// Dto to Update Group
/// </summary>
public class UpdateGroupDto : EntityDto
{
    /// <summary>
    /// Group Name
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Get or Sets customerIds
    /// </summary>
    public virtual ICollection<int> UserIds { get; set; }
}