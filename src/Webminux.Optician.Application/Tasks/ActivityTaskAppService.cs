using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician.Application.Tasks.Dtos;
using Webminux.Optician.Core.Tasks;

namespace Webminux.Optician.Application.Tasks
{
    /// <summary>
    /// Provide methods to create, assigned and get activity tasks.
    /// </summary>
    public class ActivityTaskAppService : OpticianAppServiceBase,IActivityTaskAppService
    {
        private readonly IActivityTaskManager _activityTaskManager;

        /// <summary>
        /// Create a new instance of ActivityTaskAppService.
        /// </summary>        
        public ActivityTaskAppService(IActivityTaskManager activityTaskManager)
        {
            _activityTaskManager = activityTaskManager;
        }

        /// <summary>
        /// Create a new Activity Task.
        /// </summary>
        public async Task CreateAsync(CreateActivityTaskDto input)
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            var activityTask = ActivityTask.Create(tenantId, input.Title, input.Description, input.ActivityId);
            await _activityTaskManager.CreateAsync(activityTask);
        }

        /// <summary>
        /// Assign user to task.
        /// </summary>
        public async Task AssignAsync(AssignToUserInputDto input)
        {
            await _activityTaskManager.AssignAsync(input.TaskId, input.AssignedToId);
        }

        /// <summary>
        /// Get all activity tasks.
        /// </summary>
        public async Task<ListResultDto<ActivityTaskDto>> GetAllAsync(EntityDto input)
        {
            var activityTasks = await _activityTaskManager.GetAll()
                .Where(c => c.ActivityId == input.Id)
                .Select(c => new ActivityTaskDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    AssignedToId = c.AssignedToId,
                    AssignedToName = c.User.FullName,
                    ActivityId = c.ActivityId
                }).ToListAsync();

            return new ListResultDto<ActivityTaskDto>(activityTasks);
        }
    }
}