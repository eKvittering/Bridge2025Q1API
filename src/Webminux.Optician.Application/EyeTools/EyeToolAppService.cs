using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician.Application.EyeTools.Dtos;
using Webminux.Optician.EyeTools;
using Webminux.Optician.EyeTools.Dtos;

namespace Webminux.Optician.Application.EyeTools
{
    /// <summary>
    /// Provide a service to manage the EyeTool entity.
    /// </summary>
    public class EyeToolAppService : OpticianAppServiceBase, IEyeToolAppService
    {
        private readonly IRepository<EyeTool> _repository;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EyeToolAppService(IRepository<EyeTool> repository)
        {
            _repository = repository;
        }
        #region Create
      
        /// <summary>
        /// Create a new EyeTool.
        /// </summary>
        public async Task CreateAsync(CreateEyeToolDto input)
        {
            var tenantId = AbpSession.TenantId.Value;
            var eyeTool = GetEyeTool(input, tenantId);
            await _repository.InsertAsync(eyeTool);
        }

        private static EyeTool GetEyeTool(CreateEyeToolDto input, int tenantId)
        {
            return EyeTool.Create(tenantId, input.ODRightSPH, input.ODRightCYL, input.ODRightAXIS, input.ODRightVD, input.ODRightNEARADD, input.ODRightVN, input.OSLeftSPH, input.OSLeftCYL, input.OSLeftAXIS, input.OSLeftVD, input.OSLeftNEARADD, input.OSLeftVN, input.LensType, input.LensFor, input.LensSide, input.Remark, input.ActivityId);
        }
        #endregion
      
        /// <summary>
        /// Get Eye Tool on base of Id.
        /// </summary>
        public async Task DeleteAsync(EntityDto id)
        {
            var eyeTool = await _repository.GetAsync(id.Id);

            if (eyeTool == null)
            {
                throw new EntityNotFoundException(typeof(EyeTool), id.Id);
            }
            await _repository.DeleteAsync(eyeTool);
        }

        /// <summary>
        /// Get all EyeTools.
        /// </summary>
        public async Task<ListResultDto<EyeToolDto>> GetAllAsync(EyeToolRequestDto input)
        {
            var tenantId = AbpSession.TenantId.Value;
            var query =_repository.GetAll();
            if(input.ActivityId.HasValue)
                query = query.Where(x => x.ActivityId == input.ActivityId.Value);
            var eyeTools = await query.ToListAsync();
            return new ListResultDto<EyeToolDto>(ObjectMapper.Map<List<EyeToolDto>>(eyeTools));
        }

        /// <summary>
        /// Get Eye Tool on base of Id.
        /// </summary>
        public async Task<EyeToolDto> GetAsync(EntityDto id)
        {
            var eyeTool = await _repository.FirstOrDefaultAsync(x => x.Id == id.Id);
            return ObjectMapper.Map<EyeToolDto>(eyeTool);
        }

        /// <summary>
        /// Update EyeTool.
        /// </summary>
        public async Task UpdateAsync(EyeToolDto input)
        {
            var eyeTool = await _repository.GetAsync(input.Id);
            ObjectMapper.Map(input, eyeTool);
        }
    }
}