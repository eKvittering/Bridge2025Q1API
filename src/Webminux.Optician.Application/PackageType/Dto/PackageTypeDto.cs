using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Application.Customers.Dtos;

/// <summary>
/// This is a DTO class for Groups Listing.
/// </summary>
public class PackageTypeDto : EntityDto, ICreationAudited
{
    /// <summary>
    /// set PackageType name
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// set parent id if child defaul  = 0
    /// </summary>
    public int FollowUpTypeId { get; set; }

    /// <summary>
    /// froegin key 
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    ///  froegin key 
    /// </summary>
    public int FaultId { get; set; }

    /// <summary>
    /// Gets or sets Creator User Id.
    /// </summary>
    public long? CreatorUserId { get; set; }

    /// <summary>
    /// Gets or sets Creation Time.
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    ///  tenant 
    /// </summary>
    public int TenantId { get; set; } = 0;

    /// <summary>
    /// user type id
    /// </summary>
    public int UserTypeId { get;  set; }

    /// <summary>
    ///  UserFull Name
    /// </summary>
    public string UserFullName { get;  set; }

    /// <summary>
    ///  activity type name
    /// </summary>
    public string FollowUpTypeName { get;  set; }
    public int senderTypeId { get; set; }
}