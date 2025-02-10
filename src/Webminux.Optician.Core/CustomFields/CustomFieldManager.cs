using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.CustomFields
{
    public class CustomFieldManager : ICustomFieldManager
    {
        private IRepository<CustomField> _customFieldRepository;
        private readonly IRepository<EntityFieldMapping> _entityFieldMappingRepository;

        public CustomFieldManager(
            IRepository<CustomField> customFieldRepository,
            IRepository<EntityFieldMapping> entityFieldMappingRepository)
        {
            _customFieldRepository = customFieldRepository;
            _entityFieldMappingRepository = entityFieldMappingRepository;
        }
        public async Task<List<EntityFieldMappingDto>> GetScreenCustomFields(int screen)
        {
            return await _customFieldRepository.GetAll()
                          .Select(field => new EntityFieldMappingDto
                          {
                              CustomFieldId = field.Id,
                              Label = field.Label,
                              Screen = (int)field.Screen,
                              Type = (int)field.Type,
                          })
                          .Where(field => field.Screen == screen)
                          .ToListAsync();
        }

        public async Task<List<CustomField>> GetTenantCustomFields(int tenantId)
        {
            return await _customFieldRepository.GetAllListAsync(field => field.TenantId == tenantId);
        }

        public async Task CreateEntityFieldMappings(ICollection<EntityFieldMappingDto> customFields, int tenantId, long objectId)
        {
            foreach (var field in customFields)
            {
                var entityFieldMapping = EntityFieldMapping.Create(tenantId, field.Value, objectId, field.CustomFieldId);
                await _entityFieldMappingRepository.InsertAsync(entityFieldMapping);
            }
        }

        public async Task<List<EntityFieldMappingDto>> GetObjectMappedCustomFields(long objectId, Screen screen)
        {
            return await _entityFieldMappingRepository.GetAll()
                            .Where(x => x.ObjectId == objectId && x.CustomField.Screen == screen)
                            .Select(field => new EntityFieldMappingDto
                            {
                                CustomFieldId = field.CustomFieldId,
                                ObjectId = objectId,
                                Label = field.CustomField.Label,
                                Screen = (int)field.CustomField.Screen,
                                Type = (int)field.CustomField.Type,
                                Value = field.Value
                            })
                            .ToListAsync();
        }

        public void InitializeFieldValueWithEmptyString(List<EntityFieldMappingDto> newCustomFields)
        {
            foreach (var entity in newCustomFields)
            {
                entity.Value = string.Empty;
            }
        }

        public async Task DeleteEntityFieldMappingsAsync(List<EntityFieldMapping> customFields)
        {
            foreach (var field in customFields)
            {
                await _entityFieldMappingRepository.DeleteAsync(field);
            }
        }

        public async Task<List<EntityFieldMapping>> GetEntityFieldMappingsAsync(long objectId)
        {
            return await _entityFieldMappingRepository.GetAll()
                .Where(field => field.ObjectId == objectId)
                .ToListAsync();
        }

        public bool IsHasCustomFields(List<EntityFieldMappingDto> customFields)
        {
            return customFields.Any();
        }

        public bool IsEntityHasNoCustomFieldsOrScreenHasNewCustomFields(List<EntityFieldMappingDto> entityCustomFields, List<EntityFieldMappingDto> screenCustomFields)
        {
            return IsHasCustomFields(entityCustomFields) == false || IsScreenHasNewCustomFields(entityCustomFields, screenCustomFields);
        }

        public bool IsScreenHasNewCustomFields(List<EntityFieldMappingDto> entityCustomFields, List<EntityFieldMappingDto> screenCustomFields)
        {
            return entityCustomFields.Count() != screenCustomFields.Count();
        }
    }
}
