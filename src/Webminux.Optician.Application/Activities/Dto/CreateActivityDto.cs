using System.ComponentModel.DataAnnotations;

namespace Webminux.Optician.Application.Activities.Dto
{
    public class CreateActivityDto
    {
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
        public virtual long? FollowUpByEmployeeId { get; set; }

        /// <summary>
        /// Customer Id
        /// </summary>
        public virtual long? CustomerId { get; set; }

        /// <summary>
        /// Room Id
        /// </summary>
        public virtual int? RoomId { get; set; }

        /// <summary>
        /// Decides either we need to create invite against this activity or not.
        /// </summary>
        public virtual bool IsInvited { get; set; }

        /// <summary>
        /// Id of customer in customer table
        /// </summary>
        public virtual int CustomerTableId { get; set; }

        /// <summary>
        /// Room Id
        /// </summary>
        public virtual int? GroupId { get; set; }

    }
}