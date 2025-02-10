using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using Webminux.Optician.Core.Helpers;
using Webminux.Optician.Faults.Dtos;
using Webminux.Optician.Helpers;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Faults
{
    /// <summary>
    /// Provide Methods to Get, Create and Update Status methods.
    /// </summary>
    [AbpAuthorize]
    public class FaultAppService : OpticianAppServiceBase, IFaultAppService
    {
        private readonly IRepository<Fault> _faultRepository;
        private readonly IRepository<FaultFile> _faultImageRepository;
        private readonly IActivityManager _activityManager;
        private readonly IRepository<ActivityType> _activityTypeRepository;
        private readonly IRepository<ActivityArt> _activityArtRepository;
        private readonly ICustomerManager _customerManager;
        private readonly MediaHelperService _imageHelperService;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FaultAppService(
            IRepository<Fault> faultRepository,
            IRepository<FaultFile> faultImageRepository,
            IActivityManager activityManager,
            IRepository<ActivityType> activityTypeRepository,
            IRepository<ActivityArt> activityArtRepository,
            ICustomerManager customerManager,
            MediaHelperService imageHelperService
            )
        {
            _faultRepository = faultRepository;
            _faultImageRepository = faultImageRepository;
            _activityManager = activityManager;
            _activityTypeRepository = activityTypeRepository;
            _activityArtRepository = activityArtRepository;
            _customerManager = customerManager;
            _imageHelperService = imageHelperService;
        }

        #region CreateAsync
        /// <summary>
        /// Created fault against invoice line
        /// </summary>
        /// <param name="input">CreateFaultDto with information required to create fault</param>
        /// <param name="userId">Id of user who created fault</param>
        /// <returns></returns>
        public async Task CreateAsync(CreateFaultDto input)
        {
            try
            {
                var userId = AbpSession.UserId.Value;

                var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
                var customer = await _customerManager.Customers.FirstOrDefaultAsync(customer => customer.UserId == userId);
                var responsibleEmployee = customer == null ? userId : customer.ResponsibleEmployeeId ?? userId;
                Activity activity = await CreateFaultActivity(userId, tenantId, responsibleEmployee);

                var fault = Fault.Create(tenantId, activity.Id, input.Comment, input.Description, responsibleEmployee, input.Email, input.Date.ConvertDateStringToDate(),
                     input.InvoiceLineId, input.ProductItemId, input.SupplierId, input.TicketId);
                var faultId = _faultRepository.InsertAndGetId(fault);

                await AddFilesAsync(faultId, input.Files.ToList());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// Created fault against invoice line
        /// </summary>
        /// <param name="input">CreateFaultDto with information required to create fault</param>
        /// <param name="userId">Id of user who created fault</param>
        /// <returns></returns>
        public async Task CreateFromERPAsync(CreateERPFaultDto input)
        {
            try
            {
                var userId = input.CustomerUserId;
                var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
                Activity activity = await CreateFaultActivity(userId, tenantId, input.ResposibleEmployeeId);

                //Add Activity Responsibles
                if(input.EmployeeIds != null)
                {
                    foreach (var employeeId in input.EmployeeIds)
                    {
                        var activityResponsible = ActivityResponsible.Create(activity.Id, employeeId, null);
                        await _activityManager.AddActivityResponsibleAsync(activityResponsible);
                    }
                }
                if(input.GroupIds != null)
                {
                    foreach (var groupId in input.GroupIds)
                    {
                        var activityResponsible = ActivityResponsible.Create(activity.Id, null, groupId);
                        await _activityManager.AddActivityResponsibleAsync(activityResponsible);
                    }
                }

                var fault = Fault.Create(tenantId, activity.Id, input.Comment, input.Description, input.ResposibleEmployeeId, input.Email, input.Date.ConvertDateStringToDate(),
                     null, input.ProductItemId, input.SupplierId, input.TicketId);
                var faultId = _faultRepository.InsertAndGetId(fault);

                await AddFilesAsync(faultId, input.Files.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Adds files to fault
        /// </summary>
        /// <param name="faultId">Fault Id</param>
        /// <param name="files">Failes to add against the Fault ID</param>
        public async Task AddFilesAsync(int faultId, List<CreateFaultFileDto> files)
        {
            try
            {
                // Upload images
                List<MediaUploadDto> imageUploadResult = new List<MediaUploadDto>();
                files = files
                    .Where(file => file != null && !string.IsNullOrWhiteSpace(file.Base64)).ToList();

                var uploadTasks = files
                   .Select(file => _imageHelperService.AddMediaAsync(file.Base64));

                var results = await Task.WhenAll(uploadTasks);
                imageUploadResult.AddRange(results);

                // save images
                for (int index = 0; index < imageUploadResult.Count; index++)
                {
                    var uploadedFile = imageUploadResult[index];
                    var file = files[index];
                    _faultImageRepository.Insert(FaultFile.Create(faultId, uploadedFile.PublicId, uploadedFile.Url, file.Name, file.Size, file.Type));
                }

                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Activity> CreateFaultActivity(long userId, int tenantId, long? responsibleEmployee)
        {
            ActivityType activityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activityType = await _activityTypeRepository.
                      FirstOrDefaultAsync(activityType => activityType.Name == FalutConstants.ActivityType);
                if (activityType == null)
                {
                    activityType = await CreateNewActivityType(FalutConstants.ActivityType);
                }
            }

            var activityArt = await _activityArtRepository
                .FirstOrDefaultAsync(activityArt => activityArt.Name == ActivityArtForPhoneCallActivity);

            ActivityType followUpType;
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                followUpType = await _activityTypeRepository.
               FirstOrDefaultAsync(activityType => activityType.Name == OpticianConsts.FollowUpActivityTypeForPhoneCallActivity);
                if (followUpType == null)
                {
                    followUpType = await CreateNewActivityType(OpticianConsts.FollowUpActivityTypeForPhoneCallActivity);
                }
            }


            var currentDate = DateTime.UtcNow;
            var activity = Activity.Create(tenantId,null, activityType.Name,null
                , currentDate, currentDate, activityType.Id, followUpType.Id,
                activityArt.Id, responsibleEmployee, userId, null);


            activity = await _activityManager.CreateAsync(activity);
            CurrentUnitOfWork.SaveChanges();
            return activity;
        }
        private async Task<ActivityType> CreateNewActivityType(string productItemActivityType)
        {
            var activityType = ActivityType.Create(null, productItemActivityType, 0, 0);
            activityType = await _activityTypeRepository.InsertAsync(activityType);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return activityType;
        }
        #endregion

        #region FindInvoiceLineFaults
        /// <summary>
        /// Find fault against invoice line
        /// </summary>
        /// <param name="input">Id of invoice line</param>
        /// <returns></returns>
        public async Task<ListResultDto<FaultDto>> FindInvoiceLineFaults(EntityDto input)
        {
            var query = _faultRepository.GetAll().Where(f => f.InvoiceLineId == input.Id);
            IQueryable<FaultDto> selectQuery = GetSelectQuery(query);

            var faults = await selectQuery.ToListAsync();
            return new ListResultDto<FaultDto>(faults);
        }
        #endregion

        #region GetPagedResultAsync
        /// <summary>
        /// Find paginated list of faults
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<FaultDto>> GetPagedResultAsync(PagedFaultRequestResultDto input)
        {

            var faults = ApplyFilters(input, _faultRepository.GetAll());
            var query = GetSelectQuery(faults);

            var pagedResult = await query.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            return pagedResult;
        }

        private static IQueryable<Fault> ApplyFilters(PagedFaultRequestResultDto input, IQueryable<Fault> query)
        {
            if (input.ResponsibleEmployeeId.HasValue)
                query = query.Where(fault => fault.ResponsibleEmployeeId == input.ResponsibleEmployeeId);
            if (input.CustomerUserId.HasValue)
                query = query.Where(fault => fault.CreatorUserId == input.CustomerUserId);
            if (input.ProductItemId.HasValue)
                query = query.Where(fault => fault.ProductItemId == input.ProductItemId);
            if (input.SupplierId.HasValue)
                query = query.Where(fault => fault.SupplierId == input.SupplierId);

            return query;
        }
        #endregion

        #region UpdateFaultStatus
        /// <summary>
        /// Update status of provided fault
        /// </summary>
        /// <param name="input">Contains fault status and fault Id</param>
        public async Task UpdateFaultStatus(UpdateFaultStatusDto input)
        {
            var fault = await _faultRepository.GetAsync(input.Id);
            if (fault == null)
            {
                throw new UserFriendlyException("Invalid Fault Id");
            }

            fault.Status = (OpticianConsts.FaultStatus)input.FaultStatus;
        }
        #endregion

        #region GetFaultById
        /// <summary>
        /// Get fault by Id
        /// </summary>
        /// <param name="input">Id of fault</param>
        /// <returns></returns>
        public async Task<FaultDto> GetByIdAsync(EntityDto input)
        {
            var query = _faultRepository.GetAll().Where(fault => fault.Id == input.Id);
            var selectQuery = GetSelectQuery(query);
            var fault = await selectQuery.FirstOrDefaultAsync();
            return fault;
        }
        #endregion

        #region GetByActivityIdAsync
        /// <summary>
        /// Get fault by Id
        /// </summary>
        /// <param name="input">Id of fault</param>
        /// <returns></returns>
        public async Task<FaultDto> GetByActivityIdAsync(EntityDto input)
        {
            var query = _faultRepository.GetAll().Where(fault => fault.ActivityId == input.Id);
            var selectQuery = GetSelectQuery(query);
            var fault = await selectQuery.FirstOrDefaultAsync();
            return fault;
        }
        #endregion

        #region Common Methods
        private IQueryable<FaultDto> GetSelectQuery(IQueryable<Fault> faults)
        {
            return faults.Select(f => new FaultDto
            {
                Id = f.Id,
                Comment = f.Comment,
                SolutionNote = f.SolutionNote,
                InvoiceLineId = f.InvoiceLineId,
                ProductName = f.ProductItem != null ? f.ProductItem.Product.Name : f.InvoiceLine.ProductName,
                Status = f.Status.ToString(),
                Date = f.Date,
                Email = f.Email,
                Files = f.Files.ToList(),
                InvoiceId = f.InvoiceLine.InvoiceId,
                InvoiceNo = f.InvoiceLine.Invoice.InvoiceNo,
                ResponsibleEmployeeId = f.ResponsibleEmployeeId,
                ResponsibleEmployeeName = f.ResponsibleEmployee == null ? string.Empty : f.ResponsibleEmployee.FullName,
                CreationTime = f.CreationTime,
                CreatorUserId = f.CreatorUserId,
                ActivityId = f.ActivityId ?? 0,
                ProductItemId = f.ProductItemId,
                ProductSerialNumber = f.ProductItem != null ? f.ProductItem.SerialNumber : string.Empty,
                SupplierId = f.SupplierId,
                SupplierName = f.Supplier != null ? f.Supplier.User.Name : string.Empty
            });
        }
        #endregion

        #region Update Solution Note
        public async Task SaveFaultSolution(UpdateSolutionNote note)
        {
            var fault = await _faultRepository.GetAsync(note.Id);
            if (fault == null)
            {
                throw new UserFriendlyException("Invalid Fault Id");
            }

            fault.SolutionNote = note.SolutionNote;
            await CurrentUnitOfWork.SaveChangesAsync();
        }


        #endregion
    }
}
