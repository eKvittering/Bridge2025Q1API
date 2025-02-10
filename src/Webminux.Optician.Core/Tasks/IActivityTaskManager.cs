using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Webminux.Optician.Core.Tasks
{
    public interface IActivityTaskManager : IDomainService
    {
        Task CreateAsync(ActivityTask activityTask);
        Task AssignAsync(int taskId, long assignedToId);
        IQueryable<ActivityTask> GetAll();
    }
}