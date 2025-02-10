using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.CommonDtos
{
    public class IsAlreadyExistInputDto
    {
        public long? Id { get; set; }
        public string Keyword { get; set; }
    }
}
