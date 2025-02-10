using System;
using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Activities.Dto
{
    /// <summary>
    /// Dto for Customer Notes 
    /// </summary>
    public class NoteListDto:EntityDto<int>
    {
        public virtual long? UserId { get; set; }
        public virtual string Description { get; set; }
        public DateTime CreationTime { get; set; }

        // Include properties from Activity
        public string ActivityName { get; set; }
        public int ActivityTypeId { get; set; }
    }
}