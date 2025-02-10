using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Authorization.Users;

namespace Webminux.Optician.Core.Tasks
{
    public class ActivityTask : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        [Required, StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Title { get; set; }

        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Description { get; set; }
        public virtual bool IsDone { get; set; }

        [ForeignKey(nameof(User))]
        public virtual long? AssignedToId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(Activity))]
        public virtual int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        protected ActivityTask() { }

        public static ActivityTask Create(int tenantId, string title, string description,int activityId)
        {
            return new ActivityTask
            {
                TenantId = tenantId,
                Title = title,
                Description = description,
                ActivityId = activityId,
                IsDone = false
            };
        }

    }
}