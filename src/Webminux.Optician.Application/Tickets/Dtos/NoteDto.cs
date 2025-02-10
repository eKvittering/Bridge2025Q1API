using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Customers;

namespace Webminux.Optician.Tickets.Dtos
{
    public class NoteDto : EntityDto
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }

      //  public virtual int? CustomerId { get; set; }
        public virtual long? UserId { get; set; }
        public virtual Customer Customer { get; set; }

        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public virtual int? TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual ActivityListDto ActivityDetail { get; set; }

        public virtual List<User> Employees { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual List<ActivityResponsible> ActivityResponsibles { get; set; }
    }
}
