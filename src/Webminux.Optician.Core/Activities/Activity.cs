using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Bookings;
using Webminux.Optician.Core.Notes;
using Webminux.Optician.Rooms;

namespace Webminux.Optician
{
    public class Activity : FullAuditedEntity, IMustHaveTenant, ILookupDto<int>
    {
        public virtual int TenantId { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Name { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime FollowUpDate { get; set; }

        [ForeignKey(nameof(FollowUpActivityType))]
        public virtual int? FollowUpTypeId { get; set; }
        public virtual ActivityType FollowUpActivityType { get; set; }

        
        [ForeignKey(nameof(ActivityArt))]
        public virtual int ActivityArtId { get; set; }
        public virtual ActivityArt ActivityArt { get; set; }

      
        [ForeignKey(nameof(ActivityType))]
        public virtual int ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [ForeignKey(nameof(User))]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }


        [ForeignKey(nameof(Customer))]
        public virtual long? CustomerId { get; set; }
        public virtual User Customer { get; set; }

        [ForeignKey(nameof(Group))]
        public virtual int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public virtual bool IsFollowUp { get; set; }
        public virtual bool IsClosed { get; set; }

        [ForeignKey(nameof(FollowUpByEmployee))]
        public virtual long? FollowUpByEmployeeId { get; set; }
        public virtual User FollowUpByEmployee { get; set; }

        [ForeignKey(nameof(Room))]
        public virtual int? RoomId { get; set; }
        public virtual Room Room { get; set; }

        public virtual Note Note { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual ProductItem.ProductItem ProductItem { get; set; }

    
        //  public virtual ICollection<Note> Notes { get; set; }
        protected Activity()
        {
        }

        public static Activity Create(int tenantId, long? FllowUpEmployeeId, string name, int? FllowUpGroupId, DateTime date, DateTime followUpDate, int activityTypeId, int? followUpTypeId, int activityArtId, long? employeeId, long? customerId, int? roomId, bool isClose = false)
        {
            var activity = new Activity
            {
                TenantId = tenantId,
                Name = name,
                Date = date,
                FollowUpDate = followUpDate,
                FollowUpTypeId = followUpTypeId,
                ActivityTypeId = activityTypeId,
                ActivityArtId = activityArtId,
                FollowUpByEmployeeId = (FllowUpEmployeeId.HasValue && FllowUpEmployeeId.Value > 0) ? FllowUpEmployeeId : null,
                UserId = (employeeId.HasValue && employeeId.Value > 0) ? employeeId : null,
                CustomerId = customerId,
                RoomId = roomId,
                IsClosed = isClose,
                GroupId = (FllowUpGroupId.HasValue && FllowUpGroupId.Value > 0) ? FllowUpGroupId : null
            };

            return activity;
        }



    }
}
