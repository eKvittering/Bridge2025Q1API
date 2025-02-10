using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application.EyeTools.Dtos;
using Webminux.Optician.EyeTools.Dtos;

namespace Webminux.Optician.Application.EyeTools
{

    /// <summary>
    /// Provide a service to manage the EyeTool entity.
    /// </summary>
    public interface IEyeToolAppService : IApplicationService
    {
        /// <summary>
        /// Create a new EyeTool.
        /// </summary>
        Task CreateAsync(CreateEyeToolDto input);

        /// <summary>
        /// Update a EyeTool.
        /// </summary>
        Task UpdateAsync(EyeToolDto input);

        /// <summary>
        /// Get Eye Tool on base of Id.
        /// </summary>
        Task<EyeToolDto> GetAsync(EntityDto id);

        /// <summary>
        /// Delete a EyeTool.
        /// </summary>
        Task DeleteAsync(EntityDto id);
        
        /// <summary>
        /// Get all EyeTools.
        /// </summary>
        Task<ListResultDto<EyeToolDto>>  GetAllAsync(EyeToolRequestDto input);
    }
}