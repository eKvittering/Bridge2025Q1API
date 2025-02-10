using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class EconomicApiResponseDto<T> where T : class
    {
        public List<T> collection { get; set; }
        public EconomicPaginationDto pagination { get; set; }
    }
}
