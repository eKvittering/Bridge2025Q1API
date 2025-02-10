using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Activities.Dto
{
    /// <summary>
    /// Dto to update activity
    /// </summary>
    public class UpdateActivityDto : EntityDto
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
        public virtual string Date { get; set; }

        /// <summary>
        /// Follow Up Activity Date
        /// </summary>
        public virtual string FollowUpDate { get; set; }

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
        public virtual long EmployeeId { get; set; }

        /// <summary>
        /// Customer Id
        /// </summary>
        public virtual long CustomerId { get; set; }

        /// <summary>
        /// Follow Up By Employee Id
        /// </summary>
        public virtual long? FollowUpByEmployeeId { get; set; }

        /// <summary>
        /// Room Id
        /// </summary>
        public virtual int? RoomId { get; set; }
    }
}