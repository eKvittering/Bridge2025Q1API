using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Tickets;

namespace Webminux.Optician.Activities
{
    public class ActivityResponsible : Entity
    {
        [ForeignKey(nameof(Activity))]
        public virtual int? ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        [ForeignKey(nameof(Employee))]
        public virtual long? EmployeeId { get; set; }
        public virtual User Employee { get; set; }

        [ForeignKey(nameof(Group))]
        public virtual int? GroupId { get; set; }
        public virtual Group Group { get; set; }


        public ActivityResponsible() { }

        public static ActivityResponsible Create(int activityId, long? employeeId, int? groupId)
        {
            return new ActivityResponsible
            {
                ActivityId = activityId,
                EmployeeId = employeeId,
                GroupId = groupId,
            };
        }
    }
}
