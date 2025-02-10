namespace Webminux.Optician.Application.Tasks.Dtos
{
    /// <summary>
    /// A DTO class that can be used to create a new Activity Task.
    /// </summary>
    public class CreateActivityTaskDto
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
        /// The Activity Id of the Activity Task.
        /// </summary>
        public virtual int ActivityId { get; set; }
    }
}