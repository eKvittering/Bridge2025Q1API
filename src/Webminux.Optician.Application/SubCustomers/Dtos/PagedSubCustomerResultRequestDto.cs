using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.SubCustomers.Dtos
{
    public class PagedSubCustomerResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public int CustomerId { get; set; }
    }
}
