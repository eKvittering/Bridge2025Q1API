using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Suppliers.Dtos
{
    /// <summary>
    /// This is a DTO class for <see cref="Customer"/> entity paged request.
    /// </summary>
    public class PagedSupplierResultRequestDto : PagedResultRequestDto
    {
        /// <summary>
        /// Get or Sets wild card search keyword.
        /// </summary>
        public string Keyword { get; set; }
        
    }
}