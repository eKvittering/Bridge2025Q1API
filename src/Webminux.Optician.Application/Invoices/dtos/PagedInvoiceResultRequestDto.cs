
using Abp.Application.Services.Dto;

/// <summary>
/// This is a DTO class for paged request of <see cref="Invoice"/> entity.
/// </summary>
public class PagedInvoiceResultRequestDto : PagedResultRequestDto
{

    /// <summary>
    /// Gets and sets Keyword.
    /// </summary>
    public virtual string Keyword { get; set; }

    /// <summary>
    /// Gets and Sets CustomerId.
    /// </summary>
    public virtual int? CustomerId { get; set; }

    /// <summary>
    /// User Id of customer
    /// </summary>
    public virtual long? CustomerUserId { get; set; }
}