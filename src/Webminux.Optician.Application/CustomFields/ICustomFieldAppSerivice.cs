using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.MultiTenancy.Dto;

namespace Webminux.Optician.CustomFields
{
    /// <summary>
    /// Provide Methods to operate custom fields
    /// </summary>
    public interface ICustomFieldAppSerivice:IApplicationService
    {
        /// <summary>
        /// Get Custom Fields on base of tenant
        /// </summary>
        /// <param name="input">Tenant Id</param>
        /// <returns>List of Custom Fields</returns>
        Task<ListResultDto<CustomFieldDto>> GetTenantCustomFieldsAsync(EntityDto input);

        /// <summary>
        /// Get Custom Fields of particular screen
        /// </summary>
        /// <param name="input">Screen Id</param>
        /// <returns>List of Custom Fields</returns>
        Task<ListResultDto<EntityFieldMappingDto>> GetScreenCustomFieldsAsync(EntityDto input);
    }
}
