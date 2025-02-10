using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Application.Customers.Dtos;
using Webminux.Optician.Authorization.Users;

/// <summary>
/// This is a DTO class for Groups Listing.
/// </summary>
public class PackageDto : EntityDto, ICreationAudited
{ 
    /// <summary>
    ///  employee id withour FK
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    ///  PackageId FK
    /// </summary>
    public int PackageTypeId { get; set; }
  
    /// image url
    /// </summary>
    public virtual string ImageUrl { get; set; }

    /// <summary>
    ///  customer id or supplier id
    /// </summary>
    public long? SenderId { get; set; }

    /// <summary>
    /// Gets or sets Creator User Id.
    /// </summary>
    public long? CreatorUserId { get; set; }

    /// <summary>
    /// Gets or sets Creation Time.
    /// </summary>
    public DateTime CreationTime { get; set; }
    

    /// <summary>
    /// get package type infor
    /// </summary>
    public  PackageTypeDto? packageTypeDto { get; set; }

    /// <summary>
    ///  sub package dto info
    /// </summary>
    public List<SubPackageDto>? SubPackageDtos { get; set; }

    /// <summary>
    /// sender user detal
    /// </summary>

    public string?  SenderFullName { get; set; }

  
    /// <summary>
    ///  sender  email
    /// </summary>
    public string SenderEmail { get; set; }


    /// <summary>
    ///  send User id
    /// </summary>
    public int SenderUserTypeId { get; set; }

    /// <summary>
    ///  Package Type Name
    /// </summary>
    public string PackageTypeName { get;  set; }

    /// <summary>
    /// IsOpenSubList
    /// </summary>
    public bool IsOpenSubList { get; set; } = false;

    /// <summary>
    ///  date field assing ReceiveDate
    /// </summary>
    public DateTime ReceiveDate { get;  set; }
    /// <summary>
    /// des
    /// </summary>
    public int ActivityId { get; set; }
    public string? Description { get; set; }

    public  string OuterSenderEmail { get; set; }
    public  string OuterSenderFirstName { get; set; }
    public  string OuterSenderLastName { get; set; }
    public  string OuterSenderPhoneNumber { get; set; }
    public string NoteDescription { get; set; }

    public long? FllowUpEmployeeId { get; set; }
    public int? FllowUpGroupId { get; set; }
    public virtual string FollowUpDate { get; set; }
    public virtual int? FollowUpTypeId { get; set; }
    public virtual int? ActivityTypeId { get; set; }

}