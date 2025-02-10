using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Webminux.Optician;
///<summery>
/// Dto for Invites Listing
///</summery>
public class InviteDto : CreationAuditedEntityDto
{
    ///<summery>
    /// Gets or sets the Group Id
    ///</summery>
    public virtual int? GroupId { get; set; }

    ///<summery>
    /// Gets or sets Group Name
    ///</summery>
    public virtual string GroupName { get; set; }

    ///<summery>
    /// Gets or sets the customer responses.
    ///</summery>
    public List<CustomerResponseDto> Responses { get; set; } 

    ///<summery>
    /// Gets or sets Invited Activity
    ///</summery>
    public virtual ActivityDto Activity { get; set; }

    ///<summery>
    /// Gets or sets Invited Activity Id
    ///</summery>
    public virtual int ActivityId { get; set; }

    /// <summary>
    /// Id  of invited customer
    /// </summary>
    public virtual int CustomerId { get; set; }

}