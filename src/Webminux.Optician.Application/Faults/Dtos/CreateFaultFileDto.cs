using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Faults.Dtos
{
    public class CreateFaultFileDto
    {
        public virtual string Name { get; set; }

        public virtual int Size { get; set; }

        public virtual string Type { get; set; }

        public virtual string Base64 { get; set; }
    }
}
