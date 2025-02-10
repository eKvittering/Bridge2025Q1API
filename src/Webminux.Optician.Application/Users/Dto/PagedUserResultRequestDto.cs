using Abp.Application.Services.Dto;
using System;

namespace Webminux.Optician.Users.Dto
{
    /// <summary>
    /// Paged Result request Dto for User Listing
    /// </summary>
    public class PagedUserResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? UserTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
