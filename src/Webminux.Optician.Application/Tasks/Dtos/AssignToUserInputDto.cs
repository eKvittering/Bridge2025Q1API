namespace Webminux.Optician.Application.Tasks.Dtos
{
    /// <summary>
    /// DTO to assign user to task.
    /// </summary>
    public class AssignToUserInputDto
    {
        /// <summary>
        /// The Id of the task.
        /// </summary>
        public virtual int TaskId { get; set; }
        
        /// <summary>
        /// The Id of the user.
        /// </summary>
        public virtual long AssignedToId { get; set; }
    }
}