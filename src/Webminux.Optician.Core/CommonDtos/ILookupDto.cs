using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public interface ILookupDto<TKey> : IEntityDto<TKey>
    {
        public string Name { get;  set; }
    }
}
