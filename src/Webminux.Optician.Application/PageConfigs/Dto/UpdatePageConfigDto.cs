using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.PageConfigs.Dto
{
    public class UpdatePageConfigDto : EntityDto
    {
        public string Config{ get; set; }
    }
}
