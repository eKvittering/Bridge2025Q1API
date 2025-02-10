using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Webminux.Optician
{
    public class ActivityType : Entity, ILookupDto<int>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Name { get; set; }
        public virtual int NextStepType { get; set; }
        public virtual int NextStepDage { get; set; }

        [InverseProperty(nameof(Activity.ActivityType))]
        public virtual ICollection<Activity> Activities { get; set; }

        [InverseProperty(nameof(Activity.FollowUpActivityType))]
        public virtual ICollection<Activity> FollowUpActivities { get; set; }


        protected ActivityType()
        {
        }

        public static ActivityType Create(int? tenantId, string name, int nextStepType, int nextStepDage)
        {
            var activityType = new ActivityType
            {
                Name = name,
                NextStepType = nextStepType,
                NextStepDage = nextStepDage,
                TenantId=tenantId
            };

            return activityType;
        }

        public static implicit operator ActivityType(List<ActivityType> v)
        {
            throw new NotImplementedException();
        }
    }
}
