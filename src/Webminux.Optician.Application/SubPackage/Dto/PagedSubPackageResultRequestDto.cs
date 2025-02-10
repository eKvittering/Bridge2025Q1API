using Abp.Application.Services.Dto;

/// <summary>
/// Input Dto for Group Paginated result.
/// </summary>
public class PagedSubPackageResultRequestDto : PagedResultRequestDto
{
    /// <summary>
    /// Search keyword
    /// </summary>
    public virtual string Keyword { get; set; }
}