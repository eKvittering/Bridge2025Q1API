using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician;
using Webminux.Optician.Application.Invoices.dtos;
using Webminux.Optician.CommonDtos;
using Webminux.Optician.Invoices.dtos;

/// <summary>
/// This is a interface for <see cref="Invoice"/> operations
/// </summary>
public interface IInvoiceAppService : IApplicationService
{

    /// <summary>
    /// This is a method to create <see cref="Invoice"/> entity.
    /// </summary>
    Task CreateAsync(CreateInvoiceDto input);

    /// <summary>
    /// This is a method to Update <see cref="Invoice"/> entity.
    /// </summary>
    Task UpdateAsync(InvoiceDto input);

    /// <summary>
    /// This is a method to delete <see cref="Invoice"/> entity.
    /// </summary>
    Task DeleteAsync(EntityDto<int> input);

    /// <summary>
    /// This is a method to get <see cref="Invoice"/> entities.
    /// </summary>
    Task<InvoiceDto> GetAsync(EntityDto<int> input);

    /// <summary>
    /// Provide Next Invoice Number
    /// </summary>
    /// <returns></returns>
    Task<SingleValueDto<string>> GetInvoiceNumber();

    /// <summary>
    /// This is a method to get all <see cref="Invoice"/> entities.
    /// </summary>
    Task<PagedResultDto<InvoiceDto>> GetPagedResultAsync(PagedInvoiceResultRequestDto input);

    Task<ListResultDto<LookUpDto<int>>> GetAllAsync();
    /// <summary>
    /// Add Selected Product to draft invoice
    /// </summary>
    /// <param name="input">Product detail and customer id</param>
    Task AddItemToDraftInvoice(AddItemToDraftInvoiceDto input);

    /// <summary>
    /// Save Draft Invoice
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SaveDraftInvoice(EntityDto input);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task AddItemToDraftInvoiceFromAdmin(AddItemToDraftInvoiceDto input);

    /// <summary>
    /// Save Draft Invoice for customer
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SaveDraftInvoiceForAdmin(SaveDraftInvoiceForAdminDto input);
}