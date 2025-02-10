using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Webminux.Optician
{
  /// <summary>
  /// Interface for UserType AppService
  /// </summary>
  public  interface IUserTypeAppService:IApplicationService
    {
        /// <summary>
        /// Get all UserTypes
        /// </summary>
        Task<ListResultDto<LookUpDto<int>>> GetAllAsync();

        /// <summary>
        /// Get UserType by Id
        /// </summary>
        Task<UserTypeDto> GetByIdAsync(EntityDto input);

        /// <summary>
        /// Create a new UserType
        /// </summary>
        Task CreateAsync(UserTypeDto input);

        /// <summary>
        /// Update a UserType
        /// </summary>
        Task UpdateAsync(UserTypeDto input);

        /// <summary>
        /// Delete a UserType
        /// </summary>
        Task DeleteAsync(EntityDto input);
    }
}
