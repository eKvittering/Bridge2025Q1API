using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using Webminux.Optician.Core.Helpers;
using Webminux.Optician.Tickets.Dtos;
using Webminux.Optician.Helpers;
using static Webminux.Optician.OpticianConsts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Webminux.Optician.Users;
using Webminux.Optician.Application.Users.Dto;
using System.Collections.Generic;
using System.Net.Sockets;
using Webminux.Optician.Application;
using Microsoft.EntityFrameworkCore.Internal;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Notes;
using Webminux.Optician.Faults;
using Castle.MicroKernel.Registration;
using Abp.EntityFrameworkCore.Extensions;
using Webminux.Optician.Faults.Dtos;
using Webminux.Optician.MultiTenancy;
using Webminux.Optician.Core.Customers;
using static Webminux.Optician.Authorization.Roles.StaticRoleNames;

namespace Webminux.Optician.Tickets
{
    /// <summary>
    /// Provide Methods to Get, Create and Update Status methods.
    /// </summary>
    [AbpAuthorize]
    public class TicketAppService : OpticianAppServiceBase, ITicketAppService
    {
        private readonly IRepository<Ticket> _TicketRepository;
        private readonly IRepository<TicketUser> _ticketUserRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IActivityManager _activityManager;
        private readonly IRepository<ActivityType> _activityTypeRepository;
        private readonly IRepository<ActivityArt> _activityArtRepository;
        private readonly ICustomerManager _customerManager;
        private readonly MediaHelperService _imageHelperService;
        private readonly IGroupAppService _groupAppService;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<Fault> _faultRepository;
        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<ActivityResponsible> _activityResponsibleRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        /// <summary>
        /// DeTicket Constructor
        /// </summary>
        public TicketAppService(
            IRepository<Ticket> TicketRepository,
            IRepository<TicketUser> TicketUserRepository,
            IRepository<User, long> userRepository,
            IActivityManager activityManager,
            IRepository<ActivityType> activityTypeRepository,
            IRepository<ActivityArt> activityArtRepository,
            ICustomerManager customerManager,
            MediaHelperService imageHelperService,
            IGroupAppService groupAppService,
            IUserAppService userAppService,
            IRepository<Fault> faultRepository,
            IRepository<Note> noteRepository,
            IRepository<ActivityResponsible> activityResponsible,
            IRepository<Group> groupRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<Customer> customerRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _TicketRepository = TicketRepository;
            _ticketUserRepository = TicketUserRepository;
            _userRepository = userRepository;
            _activityManager = activityManager;
            _activityTypeRepository = activityTypeRepository;
            _activityArtRepository = activityArtRepository;
            _customerManager = customerManager;
            _imageHelperService = imageHelperService;
            _groupAppService = groupAppService;
            _userAppService = userAppService;
            _faultRepository = faultRepository;
            _noteRepository = noteRepository;
            _activityResponsibleRepository = activityResponsible;
            _groupRepository = groupRepository;
            _tenantRepository = tenantRepository;
            _customerRepository = customerRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #region CreateAsync
        /// <summary>
        /// Created Ticket against invoice line
        /// </summary>
        /// <param name="input">CreateTicketDto with information required to create Ticket</param>
        /// <param name="userId">Id of user who created Ticket</param>
        /// <returns></returns>
        public async Task CreateAsync(CreateTicketDto input)
        {
            try
            {
                var userId = AbpSession.UserId.Value;
                var customerInfo = await _customerManager.Customers.Where(a => a.UserId == userId).FirstOrDefaultAsync();
                var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
                GroupDto group = await GetDefaultGroup();
                input.GroupId = group.Id;

                var activity = await CreateTicketActivity(userId, tenantId, input.EmployeeId, group.Id);

                MediaUploadDto uploadResult = new MediaUploadDto();
                if (!string.IsNullOrWhiteSpace(input.Base64ImageString))
                    uploadResult = await _imageHelperService.AddMediaAsync(input.Base64ImageString);

                Ticket Ticket = Ticket.Create(tenantId, activity.Id, input.Comment, input.Description, input.Status, input.Email, input.Date.ConvertDateStringToDate(),
                    uploadResult.PublicId, uploadResult.Url, group.Id, customerInfo.Id);
                _TicketRepository.Insert(Ticket);
                await UnitOfWorkManager.Current.SaveChangesAsync();

                int TickNumber = 10000 + Ticket.Id;
                Ticket.TicketNumber = TickNumber.ToString();

                if (input.EmployeeId != null && input.EmployeeId > 0)
                {
                    TicketUser ticketUser = TicketUser.Create(Ticket.Id, input.EmployeeId.Value);
                    await _ticketUserRepository.InsertAsync(ticketUser);
                }
                else if (input.GroupId != null && input.GroupId > 0)
                {
                    ListResultDto<UserListDto> groupUsers = await _groupAppService.GetGroupUsers(new EntityDto { Id = input.GroupId.Value });
                    foreach (var groupUser in groupUsers.Items)
                    {
                        TicketUser ticketUser = TicketUser.Create(Ticket.Id, groupUser.Id);
                        await _ticketUserRepository.InsertAsync(ticketUser);
                    }
                }

                //addTicketUsers(input, Ticket.Id);
                //await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task<GroupDto> GetDefaultGroup()
        {
            try
            {
                var group = await _groupAppService.GetGroupByName(OpticianConsts.DefautGroupName);
                if (group == null || group.Id < 1)
                {
                    var getUser = new GetAllInputDto();
                    getUser.UserType = OpticianConsts.UserTypes.Employee;
                    var getEmployees = await _userAppService.GetFilteredUsersAsync(getUser);
                    long employeeId = getEmployees.Items.Select(s => s.Id).FirstOrDefault();

                    List<int> userIds = new List<int>();
                    userIds.Add(Convert.ToInt32(employeeId));
                    CreateGroupDto groupInput = new CreateGroupDto()
                    {
                        Name = OpticianConsts.DefautGroupName,
                        UserIds = userIds
                    };

                    await _groupAppService.CreateAsync(groupInput);
                }

                group = await _groupAppService.GetGroupByName(OpticianConsts.DefautGroupName);
                return group;
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Created Ticket against invoice line
        /// </summary>
        /// <param name="input">CreateTicketDto with information required to create Ticket</param>
        /// <param name="userId">Id of user who created Ticket</param>
        /// <returns></returns>
        public async Task<int> CreateFromERPAsync(CreateERPTicketDto input)
        {
            try
            {
                int? tenantId = AbpSession.TenantId;
                GroupDto group = await GetDefaultGroup();

                if (AbpSession.TenantId == null)
                {
                    if (input.CustomerId == null || input.CustomerId <= 0)
                    {
                        if (input.TenantId == null && input.TenantId <= 0)
                            throw new UserFriendlyException("Tenant Id Not Entered.");
                        else
                        {
                            var tenant = await _tenantRepository.FirstOrDefaultAsync(x => x.Id == input.TenantId);
                            if (tenant == null)
                                throw new UserFriendlyException("Incorrect Tenant Id Entered.");

                            tenantId = tenant.Id;

                        }

                        if (string.IsNullOrWhiteSpace(input.CustomerNo))
                            throw new UserFriendlyException("Customer Number Not Entered.");
                        else
                        {
                            var customer = await _customerRepository.FirstOrDefaultAsync(x => x.CustomerNo == input.CustomerNo);
                            if (customer == null)
                                throw new UserFriendlyException("Incorrect Customer Number Entered.");

                            input.CustomerId = customer.Id;
                            input.CustomerUserId = (int)customer.UserId;
                        }
                    }
                    else
                    {
                        var customer = await _customerRepository.FirstOrDefaultAsync(x => x.Id == input.CustomerId);
                        tenantId = customer.TenantId;
                    }

                }
                else
                {
                    tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
                    input.GroupId = group.Id;
                }


                Activity activity = await CreateTicketActivity(input.CustomerUserId, tenantId.Value, input.EmployeeId, input.GroupId);
                MediaUploadDto uploadResult = new MediaUploadDto();
                if (!string.IsNullOrWhiteSpace(input.Base64ImageString))
                    uploadResult = await _imageHelperService.AddMediaAsync(input.Base64ImageString);
                Ticket Ticket = Ticket.Create(tenantId.Value, activity.Id, input.Comment, input.Description, input.Status, input.Email, input.Date.ConvertDateStringToDate(),
                    uploadResult.PublicId, uploadResult.Url, input.GroupId, input.CustomerId);
                _TicketRepository.Insert(Ticket);
                await UnitOfWorkManager.Current.SaveChangesAsync();

                int TickNumber = 10000 + Ticket.Id;
                Ticket.TicketNumber = TickNumber.ToString();
                //addTicketUsers(input, Ticket.Id);
                if (input.EmployeeId > 0)
                {
                    TicketUser ticketUser = TicketUser.Create(Ticket.Id, input.EmployeeId);
                    await _ticketUserRepository.InsertAsync(ticketUser);
                }
                else if (input.GroupId != null && input.GroupId > 0)
                {
                    ListResultDto<UserListDto> groupUsers = await _groupAppService.GetGroupUsers(new EntityDto { Id = input.GroupId.Value });
                    foreach (var groupUser in groupUsers.Items)
                    {
                        TicketUser ticketUser = TicketUser.Create(Ticket.Id, groupUser.Id);
                        await _ticketUserRepository.InsertAsync(ticketUser);
                    }
                }
                return Ticket.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Activity> CreateTicketActivity(long? customerId, int tenantId, long? employeeId, int? groupId)
        {
            try
            {
                ActivityType activityType;
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    activityType = await _activityTypeRepository.FirstOrDefaultAsync(activityType => activityType.Name == TicketConstants.ActivityType);
                    if (activityType == null)
                    {
                        activityType = await CreateNewActivityType(TicketConstants.ActivityType);
                    }
                }

                var activityArt = await _activityArtRepository.FirstOrDefaultAsync(activityArt => activityArt.Name == ActivityArtForPhoneCallActivity);

                ActivityType followUpType;
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    followUpType = await _activityTypeRepository.FirstOrDefaultAsync(activityType => activityType.Name == OpticianConsts.FollowUpActivityTypeForPhoneCallActivity);
                    if (followUpType == null)
                    {
                        followUpType = await CreateNewActivityType(OpticianConsts.FollowUpActivityTypeForPhoneCallActivity);
                    }
                }


                var currentDate = DateTime.UtcNow;
                var followDate = DateTime.UtcNow;
                if (followUpType.NextStepDage > 0)
                {
                    followDate = followDate.AddDays(followUpType.NextStepDage);
                }

                if (groupId < 1)
                {
                    groupId = null;
                }
                if (employeeId < 1)
                {
                    employeeId = null;
                }


                var activity = Activity.Create(
                    tenantId, null, activityType.Name, null, currentDate, followDate, activityType.Id, followUpType.Id,
                    activityArt.Id, employeeId, customerId, null, false);


                activity = await _activityManager.CreateAsync(activity);
                CurrentUnitOfWork.SaveChanges();
                return activity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async Task<ActivityType> CreateNewActivityType(string productItemActivityType)
        {
            var activityType = ActivityType.Create(null, productItemActivityType, 0, 0);
            activityType = await _activityTypeRepository.InsertAsync(activityType);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return activityType;
        }
        /// <summary>
        /// Find paginated list of Tickets
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TicketDto>> GetPagedResultAsync(PagedTicketRequestResultDto input)
        {
            try
            {
                var query = ApplyFiltersAsync(input);
                //var selectQuery = GetSelectQuery(query);
                var pagedResult = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                var result1 = new PagedResultDto<TicketDto>(query.Count(), pagedResult.ToList());
                return result1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        private List<TicketDto> ApplyFiltersAsync(PagedTicketRequestResultDto input)
        {
            try
            {
                string tenancyName = "";
                if (AbpSession.TenantId != null)
                {
                    var tenant = _tenantRepository.FirstOrDefault(x => x.Id == AbpSession.TenantId);
                    if (tenant != null)
                        tenancyName = tenant.TenancyName;
                }

                List<TicketDto> result = null;


                if (AbpSession.TenantId == null || tenancyName == "5000")
                {
                    using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
                    {
                        if (input.EmployeeId > 0)
                        {
                            result = (from t in _TicketRepository.GetAll()
                                      join tenant in _tenantRepository.GetAll()
                                      on t.TenantId equals tenant.Id into tenantJoin
                                      from tenant in tenantJoin.DefaultIfEmpty()
                                      select new TicketDto
                                      {
                                          Id = t.Id,
                                          Comment = t.Comment,
                                          SolutionNote = t.SolutionNote,
                                          Status = t.Status.GetEnumValueAsString(),
                                          Date = t.Date,
                                          Email = t.Email,
                                          ImagePublicKey = t.ImagePublicKey,
                                          ImageUrl = t.ImageUrl,
                                          CreationTime = t.CreationTime,
                                          CreatorUserId = t.CreatorUserId,
                                          ActivityId = t.ActivityId ?? 0,
                                          GroupId = t.GroupId,
                                          Group = t.Group,
                                          TicketNumber = t.TicketNumber,
                                          TenantId = t.TenantId,
                                          TenantName = tenant != null ? tenant.TenancyName : "Host"
                                      })
                                      .OrderByDescending(t => t.CreationTime)
                                      .ToList();

                        }
                        else
                        {
                            result = (from t in _TicketRepository.GetAll()
                                      where t.CustomerId == input.CustomerUserId
                                      join tenant in _tenantRepository.GetAll()
                                      on t.TenantId equals tenant.Id into tenantJoin
                                      from tenant in tenantJoin.DefaultIfEmpty()
                                      select new TicketDto
                                      {
                                          Id = t.Id,
                                          Comment = t.Comment,
                                          SolutionNote = t.SolutionNote,
                                          Status = t.Status.GetEnumValueAsString(),
                                          Date = t.Date,
                                          Email = t.Email,
                                          ImagePublicKey = t.ImagePublicKey,
                                          ImageUrl = t.ImageUrl,
                                          CreationTime = t.CreationTime,
                                          CreatorUserId = t.CreatorUserId,
                                          ActivityId = t.ActivityId ?? 0,
                                          GroupId = t.GroupId,
                                          Group = t.Group,
                                          TenantId = t.TenantId,
                                          TicketNumber = t.TicketNumber,
                                          TenantName = tenant != null ? tenant.TenancyName : "Host"
                                      })
                                      .OrderByDescending(t => t.CreationTime)
                                      .ToList();

                        }

                    }
                }
                else
                {
                    if (input.EmployeeId > 0)
                    {
                        result = (from t in _TicketRepository.GetAll()
                                  join tenant in _tenantRepository.GetAll()
                                     on t.TenantId equals tenant.Id into tenantJoin
                                  from tenant in tenantJoin.DefaultIfEmpty()
                                  select new TicketDto
                                  {
                                      Id = t.Id,
                                      Comment = t.Comment,
                                      SolutionNote = t.SolutionNote,
                                      Status = t.Status.GetEnumValueAsString(),
                                      Date = t.Date,
                                      Email = t.Email,
                                      ImagePublicKey = t.ImagePublicKey,
                                      ImageUrl = t.ImageUrl,
                                      CreationTime = t.CreationTime,
                                      CreatorUserId = t.CreatorUserId,
                                      ActivityId = t.ActivityId ?? 0,
                                      GroupId = t.GroupId,
                                      Group = t.Group,
                                      TicketNumber = t.TicketNumber,
                                      TenantId = t.TenantId,
                                      TenantName = tenant != null ? tenant.TenancyName : "Host"
                                  })
                                  .OrderByDescending(t => t.CreationTime)
                                  .ToList();

                    }
                    else
                    {
                        result = (from t in _TicketRepository.GetAll()
                                  where t.CustomerId == input.CustomerUserId
                                  join tenant in _tenantRepository.GetAll()
                                     on t.TenantId equals tenant.Id into tenantJoin
                                  from tenant in tenantJoin.DefaultIfEmpty()
                                  select new TicketDto
                                  {
                                      Id = t.Id,
                                      Comment = t.Comment,
                                      SolutionNote = t.SolutionNote,
                                      Status = t.Status.GetEnumValueAsString(),
                                      Date = t.Date,
                                      Email = t.Email,
                                      ImagePublicKey = t.ImagePublicKey,
                                      ImageUrl = t.ImageUrl,
                                      CreationTime = t.CreationTime,
                                      CreatorUserId = t.CreatorUserId,
                                      ActivityId = t.ActivityId ?? 0,
                                      GroupId = t.GroupId,
                                      Group = t.Group,
                                      TicketNumber = t.TicketNumber,
                                      TenantId = t.TenantId,
                                      TenantName = tenant != null ? tenant.TenancyName : "Host"
                                  })
                                  .OrderByDescending(t => t.CreationTime)
                                  .ToList();

                    }

                }




                if (!string.IsNullOrEmpty(input.Keyword))
                {
                    result = result.Where(x => x.Comment.Contains(input.Keyword) || x.Id.ToString().Contains(input.Keyword)).ToList();
                }

                if (input.ShowOnlyOpened)
                {
                    result = result.Where(x => x.Status == "Open").ToList();
                }


                var ticketIds = result.Select(t => t.Id).ToList();
                var ticketUsers = _ticketUserRepository.GetAll().Where(tu => tu.TicketId > 0 && ticketIds.Contains((int)tu.TicketId)).ToList();
                var faults = _faultRepository.GetAll()
                    .Where(fault => fault.TicketId > 0 && ticketIds.Contains((int)fault.TicketId))
                    .Include(f => f.ProductItem)
                    .Select(f => new FaultDto
                    {
                        Id = f.Id,
                        Comment = f.Comment,
                        Description = f.Description,
                        Status = f.Status.GetEnumValueAsString(),
                        ActivityId = (int)f.ActivityId,
                        ProductItemId = f.ProductItemId,
                        ProductSerialNumber = f.ProductItem.SerialNumber,
                        CreatorUserId = f.CreatorUserId,
                        CreationTime = f.CreationTime,
                        SolutionNote = f.SolutionNote,
                        InvoiceLineId = f.InvoiceLineId,
                        ProductName = f.ProductItem.Name,
                        SupplierId = f.SupplierId,
                        TicketId = f.TicketId,

                    })
                    .ToList();
                var notes = _noteRepository.GetAll()
                    .Where(note => note.TicketId > 0 && ticketIds.Contains((int)note.TicketId))
                    .Select(n => new NoteDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Description = n.Description,
                        //CustomerId = n.CustomerId,
                        UserId = n.UserId,
                        ActivityId = n.ActivityId,
                        TicketId = n.TicketId
                    })
                    .ToList();
                var noteActivityIds = notes.Select(t => t.ActivityId).ToList();
                var allNotesResponsibles = _activityResponsibleRepository.GetAll()
                    .Where(a => noteActivityIds.Contains((int)a.ActivityId))
                    .Include(a => a.Employee).Include(a => a.Group)
                    //.GroupBy(a => a.ActivityId)
                    .ToList();

                var faultActivityIds = faults.Select(t => t.ActivityId).ToList();
                var allFaultsResponsibles = _activityResponsibleRepository.GetAll()
                    .Where(a => faultActivityIds.Contains((int)a.ActivityId))
                    .Include(a => a.Employee).Include(a => a.Group)
                    //.GroupBy(a => a.ActivityId)
                    .ToList();


                foreach (var item in result)
                {
                    if (input.EmployeeId > 0)
                    {
                        item.Users = ticketUsers?.Where(n => n.TicketId == item.Id)?.Where(t => t.UserId == input.EmployeeId)?.ToList();
                    }
                    else
                    {
                        item.Users = ticketUsers?.Where(n => n.TicketId == item.Id)?.ToList();
                    }
                    item.Notes = notes?.Where(n => n.TicketId == item.Id)?.ToList();
                    item.Faults = faults?.Where(f => f.TicketId == item.Id)?.ToList();

                    foreach (var note in item.Notes)
                    {
                        var noteResponsibles = allNotesResponsibles.Where(a => a.ActivityId == note.ActivityId).ToList();
                        if (noteResponsibles.Count > 0)
                        {
                            if (noteResponsibles[0].EmployeeId > 0)
                            {
                                note.Employees = noteResponsibles.Select(a => a.Employee).ToList();
                            }
                            else
                            {
                                note.Groups = noteResponsibles.Select(a => a.Group).ToList();
                            }
                        }
                    }

                    foreach (var fault in item.Faults)
                    {
                        var faultResponsibles = allFaultsResponsibles.Where(a => a.ActivityId == fault.ActivityId).ToList();
                        if (faultResponsibles.Count > 0)
                        {
                            if (faultResponsibles[0].EmployeeId > 0)
                            {
                                fault.Employees = faultResponsibles.Select(a => a.Employee).ToList();
                            }
                            else
                            {
                                fault.Groups = faultResponsibles.Select(a => a.Group).ToList();
                            }
                        }
                    }
                }


                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion

        #region UpdateTicketStatus
        /// <summary>
        /// Update status of provided Ticket
        /// </summary>
        /// <param name="input">Contains Ticket status and Ticket Id</param>
        public async Task UpdateTicketStatus(UpdateTicketStatusDto input)
        {
            var Ticket = await _TicketRepository.GetAsync(input.Id);
            if (Ticket == null)
            {
                throw new UserFriendlyException("Invalid Ticket Id");
            }

            Ticket.Status = (OpticianConsts.TicketStatus)input.TicketStatus;
        }
        #endregion

        #region UpdateTicketUserStatus
        /// <summary>
        /// Update status of provided Ticket
        /// </summary>
        /// <param name="input">Contains Ticket status and Ticket Id</param>
        public async Task UpdateTicketUserStatus(UpdateTicketStatusDto input)
        {
            var userId = AbpSession.UserId.Value;
            var Ticket = await _ticketUserRepository.GetAll().Where(tu => tu.TicketId == input.Id && tu.UserId == userId).FirstOrDefaultAsync();
            if (Ticket == null)
            {
                throw new UserFriendlyException("Invalid Ticket Id");
            }

            Ticket.status = input.TicketStatus;
        }
        #endregion

        #region UpdateTicketStatus
        /// <summary>
        /// Update status of provided Ticket
        /// </summary>
        /// <param name="input">Contains Ticket status and Ticket Id</param>
        public async Task UpdateTicketFollowType(UpdateTicketFollowUpDto input)
        {
            var Ticket = await _TicketRepository.GetAsync(input.Id);
            if (Ticket == null)
            {
                throw new UserFriendlyException("Invalid Ticket Id");
            }

            Ticket.Activity = await _activityManager.GetAsync(Ticket.ActivityId.Value);

            // Remove previous Ticket Users
            var ticketUsersToDelete = await _ticketUserRepository.GetAll()
                .Where(tu => tu.TicketId == input.Id)
                .ToListAsync();

            if (ticketUsersToDelete.Any())
            {
                foreach (var ticketUser in ticketUsersToDelete)
                {
                    await _ticketUserRepository.DeleteAsync(ticketUser.Id);

                }
            }

            if (input.GroupId != null && input.GroupId > 0)
            {

                ListResultDto<UserListDto> groupUsers = await _groupAppService.GetGroupUsers(new EntityDto { Id = input.GroupId.Value });
                foreach (var groupUser in groupUsers.Items)
                {
                    TicketUser ticketUser = TicketUser.Create(Ticket.Id, groupUser.Id);
                    await _ticketUserRepository.InsertAsync(ticketUser);
                }
                Ticket.GroupId = input.GroupId;

                // Update group and emplactivity
                Ticket.Activity.UserId = null;
                Ticket.Activity.GroupId = input.GroupId;
            }
            else if (input.EmployeeId != null && input.EmployeeId > 0)
            {
                await _ticketUserRepository.InsertAsync(new TicketUser { TicketId = input.Id, UserId = input.EmployeeId, status = 0 });
                Ticket.GroupId = null;

                // Update group and emplactivity
                Ticket.Activity.UserId = input.EmployeeId;
                Ticket.Activity.GroupId = null;
            }

            Ticket.Activity.UserId = input.EmployeeId != null && input.EmployeeId > 0 ? input.EmployeeId : null;
            Ticket.Activity.GroupId = input.GroupId != null && input.GroupId > 0 ? input.GroupId : null;
            await _activityManager.UpdateAsync(Ticket.Activity);

            Ticket.Status = TicketStatus.Open;
            await _TicketRepository.UpdateAsync(Ticket);
            await UnitOfWorkManager.Current.SaveChangesAsync();

        }
        #endregion

        #region GetTicketById
        /// <summary>
        /// Get Ticket by Id
        /// </summary>
        /// <param name="input">Id of Ticket</param>
        /// <returns></returns>
        public async Task<TicketDto> GetByIdAsync(EntityDto input)
        {
            try
            {
                var ticketUsers = from tu in _ticketUserRepository.GetAll()
                                  join u in _userRepository.GetAll() on tu.UserId equals u.Id
                                  where tu.TicketId == input.Id
                                  select new TicketUser
                                  {
                                      TicketId = tu.TicketId,
                                      UserId = tu.UserId,
                                      status = tu.status,
                                      TicketAssignee = u
                                  };
                var tickets = _TicketRepository.GetAll().Where(t => t.Id == input.Id);
                var selectQuery = GetSelectQuery(tickets, ticketUsers);
                var Ticket = await selectQuery.FirstOrDefaultAsync();

                var faultQuery = _faultRepository.GetAll().Where(fault => fault.TicketId == input.Id);
                var faultSelectQuery = GetFaultSelectQuery(faultQuery);

                var notes = _noteRepository.GetAll()
                    .Where(note => note.TicketId > 0 && input.Id == (int)note.TicketId)
                    .Select(n => new NoteDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Description = n.Description,
                        // UserId = n.CustomerId,
                        UserId = n.UserId,
                        ActivityId = n.ActivityId,
                        TicketId = n.TicketId,
                        ActivityResponsibles = _activityResponsibleRepository.GetAll()
                                                .Where(a => a.ActivityId == Convert.ToInt32(n.ActivityId))
                                                .ToList()
                    })
                    .ToList();

                Ticket.Faults = faultSelectQuery.ToList();
                foreach (var fault in Ticket.Faults)
                {
                    fault.ActivityResponsibles = _activityResponsibleRepository.GetAll()
                                                .Where(a => a.ActivityId == Convert.ToInt32(fault.ActivityId))
                                                .ToList();
                    fault.ActivityDetail = GetActivityDto(await _activityManager.GetAsync(fault.ActivityId));

                }
                Ticket.Notes = notes.ToList();
                foreach (var nt in Ticket.Notes)
                {
                    nt.ActivityDetail = GetActivityDto(await _activityManager.GetAsync(nt.ActivityId));

                }

                return Ticket;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private static ActivityListDto GetActivityDto(Activity a)
        {
            return new ActivityListDto()
            {
                Id = a.Id,
                Name = a.Name,
                //CustomerId = a.Customer != null ? a.Customer.Customer.Id : null,
                //CustomerUserId = a.CustomerId,
                //CustomerName = a.Customer != null ? a.Customer.FullName : string.Empty,
                //CustomerEmail = a.Customer != null ? a.Customer.EmailAddress : string.Empty,
                ActivityTypeId = a.ActivityTypeId,
                //ActivityTypeName = a.ActivityType.Name,
                //ActivityArtId = a.ActivityArtId,
                //ActivityArtName = a.ActivityArt.Name,
                Date = a.Date,
                FollowUpDate = a.FollowUpDate,
                //FollowUpTypeName = a.FollowUpActivityType.Name,
                FollowUpTypeId = a.FollowUpTypeId,
                // CreationTime = a.CreationTime,
                // CreatorUserId = a.CreatorUserId,
                //EmployeeId = a.UserId,
                //EmployeeName = a.User.FullName,
                IsFollowUp = a.IsFollowUp,
                //FollowUpByEmployeeId = a.FollowUpByEmployeeId,
                //RoomId = a.RoomId,
                //RoomName = a.Room != null ? a.Room.Name : string.Empty,
                //ProductItemId = a.ProductItem != null ? a.ProductItem.Id : null,
                //ProductSerialNumber = a.ProductItem != null ? a.ProductItem.SerialNumber : string.Empty,
                //ProductName = a.ProductItem != null ? a.ProductItem.Product.Name : string.Empty,
                //ProductNumber = a.ProductItem != null ? a.ProductItem.Product.ProductNumber : string.Empty,
                //SupplierId = a.ProductItem != null ? a.ProductItem.Product.SupplierId : null,
                //SupplierName = a.ProductItem != null ? a.ProductItem.Product.Supplier != null ? a.ProductItem.Product.Supplier.User.Name : string.Empty : string.Empty,
                //GroupName = a.Group.Id > 0 ? a.Group.Name : string.Empty
            };
        }

        private static IQueryable<ActivityListDto> GetSelectQueryForActivityList(IQueryable<Activity> query)
        {
            return query.Select(a => new ActivityListDto
            {
                Id = a.Id,
                Name = a.Name,
                CustomerId = a.Customer != null ? a.Customer.Customer.Id : null,
                CustomerUserId = a.CustomerId,
                CustomerName = a.Customer != null ? a.Customer.FullName : string.Empty,
                CustomerEmail = a.Customer != null ? a.Customer.EmailAddress : string.Empty,
                ActivityTypeId = a.ActivityTypeId,
                ActivityTypeName = a.ActivityType.Name,
                ActivityArtId = a.ActivityArtId,
                ActivityArtName = a.ActivityArt.Name,
                Date = a.Date,
                FollowUpDate = a.FollowUpDate,
                FollowUpTypeName = a.FollowUpActivityType.Name,
                FollowUpTypeId = a.FollowUpTypeId,
                CreationTime = a.CreationTime,
                CreatorUserId = a.CreatorUserId,
                EmployeeId = a.UserId,
                EmployeeName = a.User.FullName,
                IsFollowUp = a.IsFollowUp,
                FollowUpByEmployeeId = a.FollowUpByEmployeeId,
                RoomId = a.RoomId,
                RoomName = a.Room != null ? a.Room.Name : string.Empty,
                ProductItemId = a.ProductItem != null ? a.ProductItem.Id : null,
                ProductSerialNumber = a.ProductItem != null ? a.ProductItem.SerialNumber : string.Empty,
                ProductName = a.ProductItem != null ? a.ProductItem.Product.Name : string.Empty,
                ProductNumber = a.ProductItem != null ? a.ProductItem.Product.ProductNumber : string.Empty,
                SupplierId = a.ProductItem != null ? a.ProductItem.Product.SupplierId : null,
                SupplierName = a.ProductItem != null ? a.ProductItem.Product.Supplier != null ? a.ProductItem.Product.Supplier.User.Name : string.Empty : string.Empty,
                GroupName = a.Group.Id > 0 ? a.Group.Name : string.Empty
            });
        }

        private IQueryable<FaultDto> GetFaultSelectQuery(IQueryable<Fault> faults)
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
                Description = f.Description,
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

        #region GetByActivityIdAsync
        /// <summary>
        /// Get Ticket by Id
        /// </summary>
        /// <param name="input">Id of Ticket</param>
        /// <returns></returns>
        public async Task<TicketDto> GetByActivityIdAsync(EntityDto input)
        {
            var ticketUsers = from tu in _ticketUserRepository.GetAll()
                              join u in _userRepository.GetAll() on tu.UserId equals u.Id
                              where tu.TicketId == input.Id
                              select new TicketUser
                              {
                                  TicketId = tu.TicketId,
                                  UserId = tu.UserId,
                                  status = tu.status,
                                  TicketAssignee = u
                              };
            var ticketQuery = _TicketRepository.GetAll().Where(Ticket => Ticket.ActivityId == input.Id);
            var selectQuery = GetSelectQuery(ticketQuery, ticketUsers);
            var Ticket = await selectQuery.FirstOrDefaultAsync();
            return Ticket;
        }
        #endregion

        #region Common Methods
        private static IQueryable<TicketDto> GetSelectQuery(IQueryable<Ticket> query, IQueryable<TicketUser> users)
        {
            return query.Select(f => new TicketDto
            {
                Id = f.Id,
                Comment = f.Comment,
                Description = f.Description,
                SolutionNote = f.SolutionNote,
                Status = f.Status.GetEnumValueAsString(),
                Date = f.Date,
                Email = f.Email,
                ImagePublicKey = f.ImagePublicKey,
                ImageUrl = f.ImageUrl,
                CreationTime = f.CreationTime,
                CreatorUserId = f.CreatorUserId,
                ActivityId = f.ActivityId ?? 0,
                GroupId = f.GroupId,
                Group = f.Group,
                TicketNumber = f.TicketNumber,
                CustomerId = f.CustomerId,
                Users = users.ToList(),
            });
        }
        #endregion

        #region Update Solution Note
        public async Task SaveTicketSolution(UpdateTicketSolutionNote note)
        {
            var Ticket = await _TicketRepository.GetAsync(note.Id);
            if (Ticket == null)
            {
                throw new UserFriendlyException("Invalid Ticket Id");
            }

            Ticket.SolutionNote = note.SolutionNote;
            await CurrentUnitOfWork.SaveChangesAsync();
        }


        #endregion
    }
}
