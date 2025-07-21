using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Webminux.Optician;
using Webminux.Optician.Companies;
using Webminux.Optician.MultiTenancy;

namespace Webminux.Optician.SynTableData
{
    public class SyncTableDataAppService : OpticianAppServiceBase, ISyncTableDataAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<BRIDGEKLUBBER> _clubRepository;
        private readonly IRepository<Tenant> _tenantRepository;

        public SyncTableDataAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<BRIDGEKLUBBER> clubRepository, IRepository<Tenant> tenantRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _clubRepository = clubRepository;
            _tenantRepository = tenantRepository;
        }
        public Task<bool> InitializeSync()
        {
            throw new NotImplementedException();
        }

        public async Task ImportClubs()
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var clubs = await _clubRepository.GetAllListAsync();

                foreach (var club in clubs)
                {
                    //var existingTenant = await _tenantRepository
                    //    .FirstOrDefaultAsync(t => t.TenancyName.ToLower() == club.Name.ToLower());
                    //if (existingTenant == null)
                    //{
                    //    var newTenant = new Tenant
                    //    {
                    //        Name = club.Name,
                    //        TenancyName = club.Name,
                    //        IsActive = true,
                    //        IsDeleted = false,
                    //        CreationTime = DateTime.UtcNow,
                    //    };

                    //    await _tenantRepository.InsertAsync(newTenant);
                    //}
                }
                await unitOfWork.CompleteAsync();
            }
        }

    }
}
