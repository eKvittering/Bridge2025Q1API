using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class EconomicSyncMapProfile : Profile
    {
        public EconomicSyncMapProfile()
        {
            CreateMap<EconomicSyncHistory, EconomicSyncHistoryDto>();
        }
    }
}
