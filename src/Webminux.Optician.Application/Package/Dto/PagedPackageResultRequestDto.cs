using Abp.Application.Services.Dto;

/// <summary>
/// Input Dto for Group Paginated result.
/// </summary>
public class PagedPackageResultRequestDto : PagedResultRequestDto
{
    public virtual string Keyword { get; set; }
    public int? PackageTypeId { get; set; } = 0;
    public string? ReceiveDate { get; set; } = "";
    public int? UserTypeId { get; set; } = 0;
    public string? ToDate { get; set; }
    public string? FromDate { get; set; }


}