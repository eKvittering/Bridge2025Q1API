using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

/// <summary>
/// Dto to Update Group
/// </summary>
public class UpdateSubPackageDto : EntityDto
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
    public virtual string Base64Picture { get; set; }

    /// <summary>
    /// group id
    /// </summary>
    public int GroupId { get; set; } = 0;

    /// <summary>
    /// employee id
    /// </summary>
    public virtual long? EmployeeId { get; set; } = 0;

}