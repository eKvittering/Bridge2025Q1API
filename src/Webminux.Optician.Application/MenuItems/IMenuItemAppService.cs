using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    internal interface IMenuItemService: IApplicationService
    {
        /// <summary>
        /// Update a Menu Item
        /// </summary>
        Task UpdateAsync(List<UpdateMenuItemDto> input);

        /// <summary>
        /// Get a Menu Items
        /// </summary>
        Task<List<MenuItemDto>> GetAsync();
    }
