using Abp.Domain.Repositories;
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
    public class ActivityTypeAppService : OpticianAppServiceBase, IActivityTypeAppService
    {
        private readonly IRepository<ActivityType> _activityTypeRepository;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ActivityTypeAppService(IRepository<ActivityType> activityTypeRepository)
        {
            _activityTypeRepository = activityTypeRepository;
        }
        /// <summary>
        /// Creates Activity Type
        /// </summary>
        /// <param name="input">Object Containing Activity Type information</param>
        public async Task CreateAsync(ActivityTypeDto input)
        {
            var activityType = ActivityType.Create(AbpSession.TenantId, input.Name, input.NextStepType, input.NextStepDage);
            await _activityTypeRepository.InsertAsync(activityType);
        }

        /// <summary>
        /// Update Activity Type 
        /// </summary>
        /// <param name="input">Activity Type object</param>
        public async Task UpdateAsync(ActivityTypeDto input)
        {
            var activityType = await _activityTypeRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, activityType);
        }
    }
}
