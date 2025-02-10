using System.Collections.Generic;
using Abp.Application.Services.Dto;

/// <summary>
/// Dto to Update Group
/// </summary>
public class UpdateCategoryDto : EntityDto
{
    /// <summary>
    /// set category name
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// set parent id if child defaul  = 0
    /// </summary>
    public int ParentCategoryId { get; set; }

    /// <summary>
    /// default activi category 
    /// </summary>
    public bool IsDeactive { get; set; }
}