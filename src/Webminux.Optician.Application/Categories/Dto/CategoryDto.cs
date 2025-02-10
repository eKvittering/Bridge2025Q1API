using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Application.Customers.Dtos;

/// <summary>
/// This is a DTO class for Groups Listing.
/// </summary>
public class CategoryDto : EntityDto, ICreationAudited
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

    /// <summary>
    /// Gets or sets Creator User Id.
    /// </summary>
    public long? CreatorUserId { get; set; }

    /// <summary>
    /// Gets or sets Creation Time.
    /// </summary>
    public DateTime CreationTime { get; set; }
}