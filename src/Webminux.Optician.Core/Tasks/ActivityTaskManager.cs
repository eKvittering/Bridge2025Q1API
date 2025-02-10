using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace Webminux.Optician.Core.Tasks
{
    public class ActivityTaskManager : IActivityTaskManager
    {
        private readonly IRepository<ActivityTask,int> _activityTaskRepository;
        public ActivityTaskManager(IRepository<ActivityTask,int> activityTaskRepository)
        {
            _activityTaskRepository = activityTaskRepository;
        }
        public async Task CreateAsync(ActivityTask activityTask)
        {
            await _activityTaskRepository.InsertAsync(activityTask);
        }

        #region Assign
        public async Task AssignAsync(int taskId, long assignedToId)
        {
            var task = await _activityTaskRepository.GetAsync(taskId);
            ValidateTask(task);

            task.AssignedToId = assignedToId;
        }

        private static void ValidateTask(ActivityTask task)
        {
            if (task == null)
                throw new Abp.UI.UserFriendlyException(OpticianConsts.ErrorMessages.TaskNotFound);
        }
        #endregion

        public IQueryable<ActivityTask> GetAll()
        {
            return _activityTaskRepository.GetAll();
        }
    }
}