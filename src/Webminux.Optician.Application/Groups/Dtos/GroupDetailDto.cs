using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Groups.Dtos
{
    /// <summary>
    /// This is a DTO class for Group Detail.
    /// </summary>
    public class GroupDetailDto : EntityDto
    {
        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets Group Users.
        /// </summary>
        public List<UserListDto> Users { get; set; }
    }
}