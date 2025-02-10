using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Tickets;

namespace Webminux.Optician.Core.Notes
{
    public class Note : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }

        //        [ForeignKey(nameof(Customer))]
        //        public virtual int? CustomerId { get; set; }
        //        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(User))]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(Activity))]
        public  int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        [ForeignKey(nameof(Ticket))]
        public virtual int? TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }


        //protected Note()
        //{
        //}

        public static Note Create(int tenantId, int? userId, string title , string description, int activityId, DateTime date, int? ticketId)
        {
            var note = new Note()
            {
                TenantId = tenantId,
              //  CustomerId = customerId,
                UserId = userId,
                Title = title,
                Description = description,
                ActivityId = activityId,
                CreationTime = date,
                TicketId = ticketId
            };

            return note;
        }
    }
}