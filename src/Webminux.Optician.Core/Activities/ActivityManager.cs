using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace Webminux.Optician.Activities
{
    public class ActivityManager : IActivityManager
    {
        private readonly IRepository<Activity> _activityRepository;
        private readonly IRepository<ActivityResponsible> _responsiblesRepository;
        private readonly IDistributedCache _distributedCache;

        private const string ActivityCacheKeyPrefix = "ActivityCache_";

        public ActivityManager(IRepository<Activity> activityRepository,
                               IRepository<ActivityResponsible> responsiblesRepository,
                               IDistributedCache distributedCache)
        {
            _activityRepository = activityRepository;
            _responsiblesRepository = responsiblesRepository;
            _distributedCache = distributedCache;
        }

        public async Task<Activity> CreateAsync(Activity activity)
        {
            return await _activityRepository.InsertAsync(activity);
        }

        public async Task<ActivityResponsible> AddActivityResponsibleAsync(ActivityResponsible activityResponsible)
        {
            return await _responsiblesRepository.InsertAsync(activityResponsible);
        }

        public async Task DeleteAsync(Activity activity)
        {
            await _activityRepository.DeleteAsync(activity);
            await _distributedCache.RemoveAsync(ActivityCacheKeyPrefix + activity.Id);
        }

        public async Task UpdateAsync(Activity activity)
        {
            await _activityRepository.UpdateAsync(activity);
            await _distributedCache.RemoveAsync(ActivityCacheKeyPrefix + activity.Id);
        }

        public async Task<Activity> GetAsync(int id)
        {
            // Define the cache key based on the id
            var cacheKey = ActivityCacheKeyPrefix + id;

            // Try to get the cached value
            var cachedActivity = await _distributedCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedActivity))
            {
                // If found in cache, deserialize and return
                return JsonSerializer.Deserialize<Activity>(cachedActivity);
            }

            // If not found, fetch from the repository
            var activity = await _activityRepository.GetAsync(id);
            if (activity != null)
            {
                // Store in cache for future use
                //var options = new DistributedCacheEntryOptions()
                //    .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Set your expiration policy
                //await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(activity), options);
                //return null;
            }

            return activity;
        }

        public IQueryable<Activity> GetAll()
        {
            return _activityRepository.GetAll();
        }

    }
}



//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Abp.Domain.Repositories;

//namespace Webminux.Optician.Activities
//{
//    public class ActivityManager : IActivityManager
//    {
//        private readonly IRepository<Activity> _activityRepository;
//        private readonly IRepository<ActivityResponsible> _responsiblesRepository;
//        public ActivityManager(IRepository<Activity> activityRepository, IRepository<ActivityResponsible> responsiblesRepository)
//        {
//            _activityRepository = activityRepository;
//            _responsiblesRepository = responsiblesRepository;
//        }

//        public async Task<Activity> CreateAsync(Activity activity)
//        {
//            try
//            {
//                return await _activityRepository.InsertAsync(activity);
//            }
//            catch (Exception)
//            {

//                throw;
//            }

//        }

//        public async Task<ActivityResponsible> AddActivityResponsibleAsync(ActivityResponsible activityResponsible)
//        {
//            try
//            {
//                return await _responsiblesRepository.InsertAsync(activityResponsible);
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }

//        public async Task DeleteAsync(Activity activity)
//        {
//            await _activityRepository.DeleteAsync(activity);
//        }

//        public async Task UpdateAsync(Activity activity)
//        {
//            await _activityRepository.UpdateAsync(activity);
//        }

//        public async Task<Activity> GetAsync(int id)
//        {
//            return await _activityRepository.GetAsync(id);
//        }

//        public IQueryable<Activity> GetAll()
//        {
//            return _activityRepository.GetAll();
//        }
//    }
//}
