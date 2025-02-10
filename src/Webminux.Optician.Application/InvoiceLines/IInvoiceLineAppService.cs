using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application.InvoiceLines.Dtos;
using Webminux.Optician.InvoiceLines.Dtos;

/// <summary>
/// This is a interface for <see cref="InvoiceLine"/> operations
/// </summary>
public interface IInvoiceLineAppService : IApplicationService
{

    /// <summary>
    /// This is a method to create <see cref="InvoiceLine"/> entity.
    /// </summary>
    Task CreateAsync(CreateInvoiceLineDto input);

    /// <summary>
    /// This is a method to Update <see cref="InvoiceLine"/> entity.
    /// </summary>
    Task UpdateAsync(InvoiceLineDto input);

    /// <summary>
    /// This is a method to delete <see cref="InvoiceLine"/> entity.
    /// </summary>
    Task DeleteAsync(EntityDto<int> input);

    /// <summary>
    /// This is a method to get <see cref="InvoiceLine"/> entities.
    /// </summary>
    Task<InvoiceLineDto> GetAsync(EntityDto<int> input);

    /// <summary>
    /// Get All invoice lines of provided invoice
    /// </summary>
    /// <param name="input">Invoice id</param>
    /// <returns>List of all invoice lines against invoice</returns>
    Task<ListResultDto<InvoiceLineDto>> GetInvoiceLinesByInvoice(EntityDto input);

    /// <summary>
    /// This is a method to get all <see cref="InvoiceLine"/> entities.
    /// </summary>
    Task<PagedResultDto<InvoiceLineDto>> GetAllAsync(GetPagedInvoiceLinesResultRequestDto input);

    /// <summary>
    /// Add Serial number to invoice line
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task AddSerialNumber(AddSerialNumberDto input);
}