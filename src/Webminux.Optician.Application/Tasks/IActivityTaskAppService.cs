using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application.Tasks.Dtos;

namespace Webminux.Optician.Application.Tasks
{
    /// <summary>
    /// A service interface that can be used to manage Activity Tasks.
    /// </summary>
    public interface IActivityTaskAppService:IApplicationService
    {
        ///<summary>
        ///Create a new Activity Task
        ///</summary>
        Task CreateAsync(CreateActivityTaskDto input);
        
        ///<summary>
        ///Assign a Activity Task to a User
        ///</summary>
        Task AssignAsync(AssignToUserInputDto input);

        ///<summary>
        ///Get all Activity Tasks
        ///</summary>
        Task<ListResultDto<ActivityTaskDto>> GetAllAsync(EntityDto input);
    }
}