using Abp.Application.Services.Dto;


/// <summary>
/// Input Dto for Invites Paginated result.
/// </summary>
public class PagedInviteResultRequestDto : PagedResultRequestDto
{
    /// <summary>
    /// Search keyword
    /// </summary>
    public virtual string Keyword { get; set; }

    /// <summary>
    /// Get or Set Customer Id to filter result. 
    /// </summary>
    public virtual int? CustomerId { get; set; }
}