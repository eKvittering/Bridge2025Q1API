using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Application.Customers.Dtos;

/// <summary>
/// This is a DTO class for Groups Listing.
/// </summary>
public class SubPackageDto : EntityDto, ICreationAudited
{


    /// <summary>
    ///  employee id withour FK
    /// </summary>
    public string Contains { get; set; }

    /// <summary>
    ///  package id
    /// </summary>
    public int PackageId { get; set; }
    /// <summary>
    ///  base img
    /// </summary>
    public  string ImageUrl { get; set; }

    /// <summary>
    /// group id
    /// </summary>
    public virtual int? GroupId { get; set; }

    /// <summary>
    /// employee id
    /// </summary>
    public virtual long? EmployeeId { get; set; } = 0;

    /// <summary>
    /// Gets or sets Creator User Id.
    /// </summary>
    public long? CreatorUserId { get; set; }

    /// <summary>
    /// Gets or sets Creation Time.
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// base64Picture
    /// </summary>
    public string Base64Picture { get; set; }
}