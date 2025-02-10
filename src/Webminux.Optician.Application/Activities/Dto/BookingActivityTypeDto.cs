using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Activities.Dto
{
    public class BookingActivityTypeDto : EntityDto, ILookupDto<int>
    {
        public string Name { get; set; }
        public int TimeInMinutes { get; set; }
        public DateTime? Duration { get; set; }
        public string Description { get; set; }

    }
}
