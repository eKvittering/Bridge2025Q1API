using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.CustomFields
{
    public interface ICustomFieldManager : IDomainService
    {
        Task<List<EntityFieldMappingDto>> GetScreenCustomFields(int screen);
        Task<List<CustomField>> GetTenantCustomFields(int tenantId);
        Task CreateEntityFieldMappings(ICollection<EntityFieldMappingDto> customFields, int tenantId, long objectId);
        Task<List<EntityFieldMappingDto>> GetObjectMappedCustomFields(long objectId, Screen screen);
        void InitializeFieldValueWithEmptyString(List<EntityFieldMappingDto> newCustomFields);
        Task DeleteEntityFieldMappingsAsync(List<EntityFieldMapping> customFields);
        Task<List<EntityFieldMapping>> GetEntityFieldMappingsAsync(long objectId);
        bool IsHasCustomFields(List<EntityFieldMappingDto> customFields);
        bool IsEntityHasNoCustomFieldsOrScreenHasNewCustomFields(List<EntityFieldMappingDto> entityCustomFields, List<EntityFieldMappingDto> screenCustomFields);
        bool IsScreenHasNewCustomFields(List<EntityFieldMappingDto> entityCustomFields, List<EntityFieldMappingDto> screenCustomFields);
    }
}
