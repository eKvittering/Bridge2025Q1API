using Abp.Application.Services.Dto;

namespace Webminux.Optician.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

