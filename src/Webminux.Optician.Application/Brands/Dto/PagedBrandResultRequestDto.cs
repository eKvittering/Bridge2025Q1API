using Abp.Application.Services.Dto;

/// <summary>
/// Input Dto for Group Paginated result.
/// </summary>
public class PagedBrandResultRequestDto : PagedResultRequestDto
{
    /// <summary>
    /// Search keyword
    /// </summary>
    public virtual string Keyword { get; set; }
}