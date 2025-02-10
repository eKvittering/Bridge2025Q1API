using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

/// <summary>
/// Dto to Update Group
/// </summary>
public class UpdatePackageDto : EntityDto
{
    public string? Description { get; set; }

    /// <summary>
    ///  employee id withour FK
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    ///  PackageId FK
    /// </summary>
    public int PackageTypeId { get; set; }
    /// <summary>
    ///  add image
    /// </summary>
    public virtual string ImagePublicKey { get; set; }
    /// <summary>
    /// image url
    /// </summary>
    public string Base64Picture { get; set; }

    /// <summary>
    ///  customer id or supplier id
    /// </summary>
    public long? SenderId { get; set; }
    /// <summary>
    /// login in employee
    /// </summary>
    ///
    public string PackageReceiveDate { get; set; }

    public int ActivityId { get; set; }
    /// <summary>
    /// Follow Up Activity Date
    /// </summary>
    ///
    public virtual int? FollowUpTypeId { get; set; }


    /// <summary>
    /// Activity Type Id
    /// </summary>
    public int? FllowUpEmployeeId { get; set; }

    /// <summary>
    /// </summary>
    public int? FllowUpGroupId { get; set; }

    public int? ActivityTypeId { get; set; }
    public string FollowUpDate { get; set; }
    public string ActivityNote { get;set; }

    //public string SenderType { get; set; }
    public string OuterSenderFirstName { get; set; }
    public string OuterSenderLastName { get; set; }
    public string OuterSenderEmail { get; set; }
    public string OuterSenderPhoneNumber { get; set; }

    public List<CreateSubPackageDto>? CreateSubPackageDtos { get; set; }
}