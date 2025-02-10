using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician;
using Webminux.Optician.MenuItems;

public class MenuItemAppService : OpticianAppServiceBase, IMenuItemService
{
    private readonly IRepository<MenuItem, int> _repository;

    /// <summary>
    /// Constructor
    /// </summary>
    public MenuItemAppService(IRepository<MenuItem, int> repository)
    {
        _repository = repository;
    
    }

    public async Task<List<MenuItemDto>> GetAsync()
    {
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
        var menuItems = await getMenuItemDtoList();
        return menuItems.Where(s => s.TenantId == tenantId).ToList();
    }

    public async Task UpdateAsync(List<UpdateMenuItemDto> input)
    {
        for(int i = 0; i < input.Count; i++)
        {
            var data = await _repository.GetAsync(input[i].Id);
            if (data == null)
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.MenuItemNotFound);

            data.Name = input[i].Name;
            data.OrderNo = input[i].OrderNo;
            data.IsActive = input[i].IsActive;

            await _repository.UpdateAsync(data);
        }
        UnitOfWorkManager.Current.SaveChanges();
    }

    private async Task<List<MenuItemDto>> getMenuItemDtoList()
    {
        return await (
            from cate in _repository.GetAll()
            select new MenuItemDto
            {
                Id = cate.Id,
                Name = cate.Name,
                OrderNo = cate.OrderNo,
                TenantId = cate.TenantId,
                CreatorUserId = cate.CreatorUserId,
                CreationTime = cate.CreationTime,
                IsActive = cate.IsActive,
                RouterLink = cate.RouteLink,
            }
        ).ToListAsync();
    }
}
