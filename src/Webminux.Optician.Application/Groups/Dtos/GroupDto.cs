using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

/// <summary>
/// This is a DTO class for Groups Listing.
/// </summary>
public class GroupDto : EntityDto, ICreationAudited
{
    /// <summary>
    /// Gets or sets the group name.
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets Creator User Id.
    /// </summary>
    public long? CreatorUserId { get; set; }

    /// <summary>
    /// Gets or sets Creation Time.
    /// </summary>
    public DateTime CreationTime { get; set; }
}