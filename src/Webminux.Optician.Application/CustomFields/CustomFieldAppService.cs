using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.MultiTenancy.Dto;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.CustomFields
{
    /// <summary>
    /// Manage Custom Fields
    /// </summary>
    public class CustomFieldAppService : OpticianAppServiceBase, ICustomFieldAppSerivice
    {
        private readonly ICustomFieldManager _customFieldManager;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="customFieldManager">Provide methods for database operations</param>
        public CustomFieldAppService(
            ICustomFieldManager customFieldManager
            )
        {
            _customFieldManager = customFieldManager;
        }

        /// <summary>
        /// Get CustomFieldType List.
        /// </summary>
        public ListResultDto<NameValueDto<int>> GetCustomFieldTypes()
        {
            var customFields = Enum.GetValues(typeof(CustomFieldType)).Cast<CustomFieldType>().Select(x => new NameValueDto<int>
            {
                Name = x.ToString(),
                Value = (int)x
            }).ToList();

            return new ListResultDto<NameValueDto<int>>(customFields);
        }

        /// <summary>
        /// Get Screens List.
        /// </summary>
        public ListResultDto<NameValueDto<int>> GetScreens()
        {
            var customFields = Enum.GetValues(typeof(Screen)).Cast<Screen>().Select(x => new NameValueDto<int>
            {
                Name = x.ToString(),
                Value = (int)x
            }).ToList();

            return new ListResultDto<NameValueDto<int>>(customFields);
        }

        /// <summary>
        /// Get All Custom Fields of current Tenant
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<CustomFieldDto>> GetTenantCustomFieldsAsync(EntityDto input)
        {
            var customFields = await _customFieldManager.GetTenantCustomFields(input.Id);
            return new ListResultDto<CustomFieldDto>(ObjectMapper.Map<List<CustomFieldDto>>(customFields));
        }

        #region Get Screen Custom Field
        /// <summary>
        /// Get Custom Fields of particular screen.
        /// </summary>
        /// <param name="input">Screen Id</param>
        /// <returns></returns>
        public async Task<ListResultDto<EntityFieldMappingDto>> GetScreenCustomFieldsAsync(EntityDto input)
        {
            List<EntityFieldMappingDto> customFields = await _customFieldManager.GetScreenCustomFields(input.Id);
            return new ListResultDto<EntityFieldMappingDto>(customFields);
        }

        #endregion
    }
}
