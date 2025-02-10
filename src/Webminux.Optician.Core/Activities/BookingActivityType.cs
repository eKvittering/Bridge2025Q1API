using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Activities
{
    public class BookingActivityType : Entity, ILookupDto<int>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Name { get; set; }
        public int TimeInMinutes { get; set; }
        public DateTime? Duration { get; set; }
        public string Description { get; set; }

    }
}
