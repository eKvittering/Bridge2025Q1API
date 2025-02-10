using Abp.Application.Services.Dto;

/// <summary>
/// Input Dto for Group Paginated result.
/// </summary>
public class PagedProductItemResultRequestDto : PagedResultRequestDto
{
    /// <summary>
    /// Search keyword
    /// </summary>
    public virtual string Keyword { get; set; }
    public bool? IsAvailable { get; set; }
    public bool? IsMedicalDevice { get; set; }
    public int? ProductId { get; set; }
    public string SerialNo { get; set; }
    public long? ReceiverEmployee { get; set; }
    public string ReciverDate { get; set; }
}