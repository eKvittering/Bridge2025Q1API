using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Tickets
{
    public class TicketUser: Entity
    {
        [ForeignKey(nameof(Ticket))]
        public virtual int? TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        [ForeignKey(nameof(TicketAssignee))]
        public virtual long? UserId { get; set; }
        public virtual User TicketAssignee { get; set; }

        public virtual int status { get; set; }

        public TicketUser() { }

        public static TicketUser Create(int ticketId, long userId)
        {
            return new TicketUser
            {
                TicketId = ticketId,
                UserId = userId,
                status = 0, // Pending
            };
        }
    }
}
