using Abp.Application.Services.Dto;


/// <summary>
/// Input Dto for Invites Paginated result.
/// </summary>
public class PagedRoomResultRequestDto : PagedResultRequestDto
{
    /// <summary>
    /// Search keyword
    /// </summary>
    public virtual string Keyword { get; set; }
}