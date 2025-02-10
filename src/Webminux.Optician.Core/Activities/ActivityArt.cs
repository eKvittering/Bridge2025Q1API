using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Webminux.Optician
{
    public class ActivityArt : Entity,ILookupDto<int>
    {

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Name { get; set; }

        protected ActivityArt()
        {
        }

        public static ActivityArt Create(int tenantId,string name)
        {
            var activityArt = new ActivityArt
            {
                Name = name
            };

            return activityArt;
        }
    }
}
