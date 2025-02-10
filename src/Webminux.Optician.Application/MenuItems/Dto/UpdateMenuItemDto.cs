using System.Collections.Generic;
using Abp.Application.Services.Dto;

/// <summary>
/// Dto to Update Group
/// </summary>
public class UpdateMenuItemDto : EntityDto
{
    /// <summary>
    /// set Menu Item name
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// set Order No
    /// </summary>
    public int OrderNo { get; set; }

    /// <summary>
    /// Is Menu Item Active?
    /// </summary>
    public bool IsActive { get; set; }
}