using Abp.Application.Services.Dto;

namespace Webminux.Optician.Products.Dto
{
    /// <summary>
    ///  for pagination
    /// </summary>
    public class PagedProductGroupResultRequestDto : PagedResultRequestDto
    {
        /// <summary>
        /// Wild Card Keyword
        /// </summary>
        public string Keyword { get; set; }
    }
}
