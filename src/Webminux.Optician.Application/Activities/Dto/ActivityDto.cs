using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Webminux.Optician
{
    /// <summary>
    /// Transfer data for Activity Listing
    /// </summary>
    public class ActivityDto : EntityDto, ICreationAudited
    {
        /// <summary>
        /// Tenant Id of the activity
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// Name of Activity
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Date of Activity
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Follow Up Activity Date
        /// </summary>
        public virtual DateTime FollowUpDate { get; set; }

        /// <summary>
        /// Follow Up Activity Type Id 
        /// </summary>
        public virtual int? FollowUpTypeId { get; set; }

        /// <summary>
        /// Activity Art Id
        /// </summary>
        public virtual int ActivityArtId { get; set; }

        /// <summary>
        /// Activity Type Id
        /// </summary>
        public virtual int ActivityTypeId { get; set; }

        /// <summary>
        /// Assigned Employee Id
        /// </summary>
        public virtual long? EmployeeId { get; set; }

        /// <summary>
        /// Customer User Id
        /// </summary>
        public virtual long? CustomerId { get; set; }

        /// <summary>
        /// Creator User Id
        /// </summary>
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// Creation Time
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Decide if the activity is follow up
        /// </summary>
        public virtual bool IsFollowUp { get; set; }

        /// <summary>
        /// Follow Up By Employee Id
        /// </summary>
        public virtual long? FollowUpByEmployeeId { get; set; }

        /// <summary>
        /// Room Id
        /// </summary>
        public virtual int? RoomId { get; set; }

        /// <summary>
        /// Note of Email,Phone and SMS Activity
        /// </summary>
        public virtual string Note { get; set; }
    }
}
