using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using Webminux.Optician.Application.Activities.Dto;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Notes;
using Webminux.Optician.Faults;
using Webminux.Optician.Faults.Dtos;

namespace Webminux.Optician.Tickets.Dtos
{
    
    public class TicketDto : EntityDto, ICreationAudited
    {
        public virtual string Comment { get; set; }
        public virtual string Description{ get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Base64ImageString { get; set; }
        public virtual string ImagePublicKey { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Status { get; set; }
        public virtual int ActivityId { get; set; }
        public virtual int? GroupId { get; set; }
        public virtual long? EmployeeId { get; set; }
        public virtual long? CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual string SolutionNote { get; set; }
        public virtual string TicketNumber { get; set; }
        public virtual Group Group { get; set; }
        public virtual List<TicketUser> Users { get; set; }
        public virtual bool IsExpanded { get; set; }
        public virtual int? CustomerId { get; set; }
        public virtual int? TenantId { get; set; }
        public virtual List<FaultDto> Faults { get; set; }
        public virtual List<NoteDto> Notes { get; set; }

    }
}
