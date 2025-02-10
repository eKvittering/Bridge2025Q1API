using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Customers;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Tickets
{
    public class Ticket : CreationAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        /// <summary>
        /// Information provided by client about Ticket
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Comment { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxDescriptionLength)]
        public virtual string SolutionNote { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Email { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual TicketStatus Status { get; set; }
        public virtual string ImagePublicKey { get; set; }
        public virtual string ImageUrl { get; set; }

        [ForeignKey(nameof(TicketUserCustomer))]
        public virtual int? CustomerId { get; set; }
        public virtual Customer TicketUserCustomer { get; set; }

        [ForeignKey(nameof(Group))]
        public virtual int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        [ForeignKey(nameof(Activity))]
        public virtual int? ActivityId { get; set; }
        public virtual Activity Activity { get; set; }


        public string TicketNumber { get; set; }


        protected Ticket() { }

        public static Ticket Create(int tenantId, int activityId, string comment, string description, int status, string email, DateTime date, string imagePublicKey, string imageUrl, int? GroupId, int? CustomerId)
        {

            //string datePart = DateTime.Now.ToString("yyMMddHHmm");
            //string guidPart = Guid.NewGuid().ToString("N").Substring(0, 5); ;
            //string uniqueTicketNumber = $"{datePart}-{guidPart}";
            //string number =  uniqueTicketNumber;

            return new Ticket
            {
                TenantId = tenantId,
                Date = date,
                Email = email,
                ImagePublicKey = imagePublicKey,
                ImageUrl = imageUrl,
                Comment = comment,
                Description = description == null ? String.Empty : description,
                Status = (TicketStatus)Enum.Parse(typeof(TicketStatus), status.ToString()),
                ActivityId = activityId,
                SolutionNote = string.Empty,
                GroupId = (GroupId > 0) ? GroupId : null,
                CustomerId = CustomerId,
                // TicketNumber = number

            };
        }



    }
}
