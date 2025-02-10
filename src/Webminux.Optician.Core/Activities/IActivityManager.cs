using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Webminux.Optician.Activities
{
    public interface IActivityManager : IDomainService
    {
        Task<Activity> CreateAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(Activity activity);
        Task<Activity> GetAsync(int id);       
        IQueryable<Activity> GetAll();
        Task<ActivityResponsible> AddActivityResponsibleAsync(ActivityResponsible activityResponsible);

    }
}
