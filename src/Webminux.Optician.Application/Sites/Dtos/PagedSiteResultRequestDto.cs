using Abp.Application.Services.Dto;

/// <summary>
/// Input Dto for Site Paginated result.
/// </summary>
public class PagedSiteResultRequestDto : PagedResultRequestDto
{
    /// <summary>
    /// Search keyword
    /// </summary>
    public virtual string Keyword { get; set; }
    public  int tenantId { get; set; }
}
