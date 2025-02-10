using System.Collections.Generic;
using Abp.Application.Services.Dto;

/// <summary>
/// Dto to Update Group
/// </summary>
public class UpdatePackageTypeDto : EntityDto
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
    public int senderTypeId { get; set; }
}