using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Webminux.Optician.Core;
using Webminux.Optician.Helpers;

namespace Webminux.Optician
{
    /// <summary>
    /// Provides methods to manage activities
    /// </summary>
    public class UserTypeAppService : OpticianAppServiceBase, IUserTypeAppService
    {
        private readonly IRepository<UserType> _userTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserTypeAppService(IRepository<UserType> userTypeRepository)
        {
            _userTypeRepository = userTypeRepository;
        }

        /// <summary>
        /// Get all UserTypes
        /// </summary>
        public async Task<ListResultDto<LookUpDto<int>>> GetAllAsync()
        {
            var userTypes = await _userTypeRepository.GetAll().GetLookupResultAsync<UserType, int>();
            return userTypes;
        }

        /// <summary>
        /// Get UserType by Id
        /// </summary>
        public async Task<UserTypeDto> GetByIdAsync(EntityDto input)
        {
            var userType = await _userTypeRepository.GetAsync(input.Id);
            var userTypeDto = ObjectMapper.Map<UserTypeDto>(userType);
            return userTypeDto;
        }

        /// <summary>
        /// Create a new UserType
        /// </summary>
        public async Task CreateAsync(UserTypeDto input)
        {
            var userType = UserType.Create(input.Name);
            await _userTypeRepository.InsertAsync(userType);
        }

        /// <summary>
        /// Update a UserType
        /// </summary>
        public async Task UpdateAsync(UserTypeDto input)
        {
            var userType = await _userTypeRepository.GetAsync(input.Id);
            userType.Name = input.Name;
            await _userTypeRepository.UpdateAsync(userType);
        }

        /// <summary>
        /// Delete a UserType
        /// </summary>
        public async Task DeleteAsync(EntityDto input)
        {
            var userType = await _userTypeRepository.GetAsync(input.Id);
            await _userTypeRepository.DeleteAsync(userType);
        }


    }
}
