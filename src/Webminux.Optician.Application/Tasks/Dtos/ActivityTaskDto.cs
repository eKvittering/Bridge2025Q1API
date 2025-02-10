using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Tasks.Dtos
{
    /// <summary>
    /// A DTO class that can be used to Activity Task.
    /// </summary>
    public class ActivityTaskDto:EntityDto
    {
        /// <summary>
        /// The Title of the Activity Task.
        /// </summary>
        public virtual string Title { get; set; }
        
        /// <summary>
        /// The Description of the Activity Task.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// The User Id of the User assigned to  Activity Task.
        /// </summary>
        public virtual long? AssignedToId { get; set; }
        
        /// <summary>
        /// The Name of the User assigned to  Activity Task.
        /// </summary>
        public virtual string AssignedToName { get; set; }
        
        /// <summary>
        /// The Activity Id of the Activity Task.
        /// </summary>
        public virtual int ActivityId { get; set; }
        
        /// <summary>
        /// Provide information about task completion.
        /// </summary>
        public virtual bool IsDone { get; set; }
    }
}