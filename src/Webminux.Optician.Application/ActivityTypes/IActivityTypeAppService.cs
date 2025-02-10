using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ActivityTypes
{
    /// <summary>
    /// Provide Create, Update, Delete and Get methods for Activity Type
    /// </summary>
    public interface IActivityTypeAppService:IApplicationService
    {
        /// <summary>
        /// Creates Activity Type
        /// </summary>
        /// <param name="input">Object Containing Activity Type information</param>
        Task CreateAsync(ActivityTypeDto input);

        /// <summary>
        /// Update Activity Type 
        /// </summary>
        /// <param name="input">Activity Type object</param>
        Task UpdateAsync(ActivityTypeDto input);


        
    }
}
