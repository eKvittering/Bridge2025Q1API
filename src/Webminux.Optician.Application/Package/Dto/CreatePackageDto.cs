using System.Collections.Generic;

/// <summary>
/// Dto to Create Group
/// </summary>
public class CreatePackageDto
{
   
  
    /// <summary>
    ///  employee id withour FK
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    ///  PackageId FK
    /// </summary>
    public int PackageTypeId { get; set; }

    /// <summary>
    /// Base64 String of uploaded picture
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
    public string? PackageReceiveDate { get; set; }


    /// <summary>
    /// Follow Up Activity Date
    /// </summary>
    public virtual string FollowUpDate { get; set; }

    /// <summary>
    /// Follow Up Activity Type Id 
    /// </summary>
    public virtual int? FollowUpTypeId { get; set; }


    /// <summary>
    /// Activity Type Id
    /// </summary>
    public virtual int? ActivityTypeId { get; set; }



    /// <summary>
    /// Activity Type Id
    /// </summary>
    /// 

    //ActivityID
    public int ActivityId { get; set; }

    public int? FllowUpEmployeeId { get; set; }

    /// <summary>
    /// </summary>
    public  int? FllowUpGroupId { get; set; }

    public string? Description { get; set; }
    public string? ActivityNote { get; set; }

    //Costumer supplier Detail

    // public string SenderType { get; set; } 
    public string OuterSenderEmail { get; set; }
    public string OuterSenderFirstName { get; set; }
    public string OuterSenderLastName { get; set; }
    public string OuterSenderPhoneNumber { get; set; }
    public List<CreateSubPackageDto>? CreateSubPackageDtos { get; set; }

}

