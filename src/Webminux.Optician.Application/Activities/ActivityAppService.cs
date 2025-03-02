using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Webminux.Optician.Activities.Dto;
using Webminux.Optician.Application;
using Webminux.Optician.Application.Activities.Dto;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.Notes;
using Webminux.Optician.Helpers;
using Webminux.Optician.Tickets;

namespace Webminux.Optician.Activities
{
    /// <summary>
    /// Provides methods to manage activities
    /// </summary>
    [AbpAuthorize()]
    public class ActivityAppService : OpticianAppServiceBase, IActivityAppService
    {
        private readonly IRepository<ActivityArt> _activityArtRepository;
        private readonly IRepository<ActivityType> _activityTypeRepository;
        private readonly IInviteManager _inviteManager;
        private readonly IRepository<Note> _noteRepository;
        private readonly IProductItemAppService _productItemAppService;
        private readonly IActivityManager _activityManager;
        private readonly IRepository<BookingActivityType> _bookingActivityTypeRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<ActivityResponsible> _activityResponsibleRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityAppService(
            IActivityManager activityManager,
            IRepository<ActivityArt> activityArtRepository,
            IRepository<ActivityType> activityTypeRepository,
            IInviteManager inviteManager,
            IRepository<Note> noteRepository,
            IProductItemAppService productItemAppService,
            IRepository<BookingActivityType> bookingActivityTypeRepository,
            IRepository<Customer> customerRepository,
            IRepository<Ticket> ticketRepository,
            IRepository<ActivityResponsible> activityResponsibleRepository
            )
        {
            _activityManager = activityManager;
            _activityArtRepository = activityArtRepository;
            _activityTypeRepository = activityTypeRepository;
            _inviteManager = inviteManager;
            _noteRepository = noteRepository;
            _productItemAppService = productItemAppService;
            _bookingActivityTypeRepository = bookingActivityTypeRepository;
            _customerRepository = customerRepository;
            _ticketRepository = ticketRepository;
            _activityResponsibleRepository = activityResponsibleRepository;
        }

        #region Create
        /// <summary>
        /// Create a new activity
        /// </summary>
        public async Task CreateAsync(CreateActivityDto input)
        {
            int tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            Activity activity = GetActivityModel(input, tenantId);
            _ = await _activityManager.CreateAsync(activity);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (IsInvitedActivity(input))
            {
                await CreateInviteAsync(input, tenantId, activity);
            }
        }

        private static bool IsInvitedActivity(CreateActivityDto input)
        {
            return input.IsInvited;
        }

        private async Task CreateInviteAsync(CreateActivityDto input, int tenantId, Activity activity)
        {
            Invite invite = Invite.Create(tenantId, null, activity.GroupId, activity.Id);
            if (invite.CustomerId == 0)
                invite.CustomerId = null; await _inviteManager.CreateAsync(invite);
        }
        #endregion

        /// <summary>
        /// Update an existing activity
        /// </summary>
        public async Task UpdateAsync(UpdateActivityDto input)
        {
            Activity activityFromDb = await _activityManager.GetAsync(input.Id);
            if (activityFromDb == null)
            {
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.ActivityNotFound);
            }
            _ = ObjectMapper.Map(input, activityFromDb);
        }

        /// <summary>
        /// Delete an existing activity
        /// </summary>
        public async Task DeleteAsync(EntityDto input)
        {
            Activity activity = await _activityManager.GetAsync(input.Id);
            if (activity == null)
            {
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.ActivityNotFound);
            }

            await _activityManager.DeleteAsync(activity);
        }

        /// <summary>
        /// Get an activity by id
        /// </summary>
        public async Task<ActivityDto> GetAsync(EntityDto input)
        {
            IQueryable<Activity> query = _activityManager.GetAll();
            query = query.Where(a => a.Id == input.Id);
            IQueryable<ActivityDto> selectQuery = GetSelectQueryForGetActivity(query);
            return await selectQuery.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get all activities
        /// </summary>
        public async Task<ListResultDto<LookUpDto<int>>> GetAllAsync()
        {
            return await _activityManager.GetAll().GetLookupResultAsync<Activity, int>();
        }

        /// <summary>
        /// Get all activities by date filter
        /// </summary>
        public async Task<ListResultDto<ActivityListDto>> GetAllActivitiesAsync(GetActivitiesInputDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                int? tenantId = AbpSession.TenantId;
                DateTime fromDate = DateTime.ParseExact(input.FromDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(input.ToDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture);
                IQueryable<Activity> query = _activityManager.GetAll();

                query = ApplyDateRangeWithRoomFilter(input, fromDate, toDate, query);
                IQueryable<ActivityListDto> selectQuery = GetSelectQueryForActivityList(query);

                ListResultDto<ActivityListDto> result = new(await selectQuery.ToListAsync());
                return result;
            }
        }



        /// <summary>
        /// activities with performance
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<ActivityListDto>> GetPagedResultAsync(PagedActivityRequestResultDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                int? tenantId = AbpSession.TenantId;
                var query = _activityManager.GetAll().AsNoTracking();  // Add AsNoTracking for performance boost
                var totalCount = await query.CountAsync();
                if (input.CustomerId.HasValue)
                {
                    var customerIds = _customerRepository.GetAll()
                        .AsNoTracking()
                        .Where(c => c.ParentCustomer.UserId == input.CustomerId)
                        .Select(x => x.UserId)
                        .ToList();

                    query = query.Where(a => a.CustomerId == input.CustomerId || customerIds.Contains(a.CustomerId ?? 0));
                }

                if (input.IsFollowUp.HasValue)
                {
                    query = query.Where(a => a.IsFollowUp == input.IsFollowUp);
                }

                if (input.IsClosed.HasValue)
                {
                    query = query.Where(a => a.IsClosed == input.IsClosed);
                }

                if (tenantId.HasValue)
                {
                    query = query.Where(a => a.TenantId == tenantId);
                }

                if (input.FollowUpActivityTypeId.HasValue)
                {
                    query = query.Where(a => a.FollowUpTypeId == input.FollowUpActivityTypeId);
                }

                if (!string.IsNullOrEmpty(input.Keyword))
                {
                    query = query.Where(a => string.Concat(a.Customer.Name, " ", a.Customer.Surname).Contains(input.Keyword)
                                              || a.Name.Contains(input.Keyword) || a.ActivityType.Name.Contains(input.Keyword)
                                              || string.Concat(a.User.Name, " ", a.User.Surname).Contains(input.Keyword)
                                              || a.ActivityArt.Name.Contains(input.Keyword) || a.Customer.EmailAddress.Contains(input.Keyword)
                                              || a.Customer.PhoneNumber.Contains(input.Keyword) || a.Customer.UserName.Contains(input.Keyword)
                                              || a.Name.Contains(input.Keyword));
                }

                if (input.IsFromRememberReport && input.IsOnlyMe == true)
                {
                    long? currentUserId = AbpSession.UserId;
                    query = query.Where(a => a.UserId == currentUserId);
                }

                var activities = await query
                    .OrderByDescending(a => a.CreationTime) 
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .Select(a => new ActivityListDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        CustomerId = a.CustomerId != null ? a.CustomerId : null,
                        CustomerUserId = a.CustomerId,
                        CustomerName = a.Customer != null ? a.Customer.FullName : string.Empty,
                        CustomerEmail = a.Customer != null ? a.Customer.EmailAddress : string.Empty,
                        CustomerType = a.Customer != null ? a.Customer.Customer.CustomerType.Type : string.Empty,
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
                        Note = a.Note.Description != null ? a.Note.Description.ToString() : string.Empty,
                        RoomName = a.Room != null ? a.Room.Name : string.Empty,
                        ProductItemId = a.ProductItem != null ? a.ProductItem.Id : null,
                        ProductSerialNumber = a.ProductItem != null ? a.ProductItem.SerialNumber : string.Empty,
                        ProductName = a.ProductItem != null ? a.ProductItem.Product.Name : string.Empty,
                        ProductNumber = a.ProductItem != null ? a.ProductItem.Product.ProductNumber : string.Empty,
                        SupplierId = a.ProductItem != null ? a.ProductItem.Product.SupplierId : null,
                        SupplierName = a.ProductItem != null ? a.ProductItem.Product.Supplier != null ? a.ProductItem.Product.Supplier.User.Name : string.Empty : string.Empty,
                        GroupName = a.Group.Id > 0 ? a.Group.Name : string.Empty,
                    }).ToListAsync();



                return new PagedResultDto<ActivityListDto>(totalCount, activities);
            }
        }


        /// <summary>
        /// Get all activity types
        /// </summary>
        public async Task<ListResultDto<LookUpDto<int>>> GetAllActivityTypesAsync()
        {
            using IDisposable unitOfWork = UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            IQueryable<ActivityType> query = _activityTypeRepository.GetAll();
            query = AbpSession.TenantId.HasValue
                ? query.Where(at => at.TenantId == null || at.TenantId == AbpSession.TenantId.Value)
                : query.Where(at => at.TenantId == null);

            ListResultDto<LookUpDto<int>> result = await query.GetLookupResultAsync<ActivityType, int>();
            return result;
        }

        /// <summary>
        /// Get all activity arts
        /// </summary>
        public async Task<ListResultDto<LookUpDto<int>>> GetAllActivityArtsAsync()
        {
            try
            {
                return await _activityArtRepository.GetAll().GetLookupResultAsync<ActivityArt, int>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// Mark Activity As Follow Up By Current User
        /// </summary>
        public async Task MarkActivityAsFollowUpAsync(EntityDto input)
        {
            long currentUserId = AbpSession.UserId.Value;
            Activity activity = await _activityManager.GetAsync(input.Id);
            if (activity == null)
            {
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.ActivityNotFound);
            }
            activity.IsFollowUp = true;
            activity.FollowUpByEmployeeId = currentUserId;
        }

        /// <summary>
        /// Create Phone Call Note Activity and Add Note
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreatePhoneCallNoteActivityAndAddNote(NoteActivityInputDto input)
        {
            try
            {
                Activity activity = await CreateNoteActivityAsync(input, OpticianConsts.PhoneCallActivityType);
                await CurrentUnitOfWork.SaveChangesAsync();
                await CreateNote(input, activity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Create SMS Note Activity and Add Note
        /// </summary>
        /// <param name="input">Object Contain Customer Id and Note Description</param>`
        /// <returns></returns>
        public async Task CreateSmsNoteActivityAndAddNote(NoteActivityInputDto input)
        {
            try
            {
                Activity activity = await CreateNoteActivityAsync(input, OpticianConsts.SmsNoteActivityType);
                await CurrentUnitOfWork.SaveChangesAsync();
                await CreateNote(input, activity);
            }
            catch (Exception e)
            {

                throw;
            }


        }
        /// <inheritdoc/>
        public async Task SendMessageAsync(string message, string phoneNumber)
        {
            try
            {
                RestClientOptions options = new("https://api.cpsms.dk");
                RestClient client = new(options);
                RestRequest request = new("/v2/send", Method.Post);
                _ = request.AddHeader("Content-Type", "application/json");
                _ = request.AddHeader("Authorization", "Basic YmVja2l0OmIxYTIwYWM2LWQ2NzEtNDkzYi04NzRiLTI2Y2Y1YzUzNjk1Mg==");
                string body = @"{
                " + "\n" +
                                @"
                " + "\n" +
                                @"""to"":""" + phoneNumber + @""", ""message"": """ + message + @""", ""from"": ""Beck IT""
                " + "\n" +
                                @"
                " + "\n" +
                @"}";
                _ = request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Create SMS Note Activity and Add Note
        /// </summary>
        /// <param name="input">Object Contain Customer Id and Note Description</param>
        /// <returns></returns>
        public async Task CreateEmailNoteActivityAndAddNote(NoteActivityInputDto input)
        {
            Activity activity = await CreateNoteActivityAsync(input, OpticianConsts.EmailNoteActivityType);
            await CurrentUnitOfWork.SaveChangesAsync();
            await CreateNote(input, activity);

        }
        /// <inheritdoc/>
        /// 

        public void SendMail(string to, string subject, string body)
        {

            string email = "carecenter.alert-it@beckcrm.dk";
            string password = "carecenter.alert-itqwe123QWE";

            string smtpServer = "smtp.simply.com";
            int smtpPort = 587;


            MailMessage mail = new()
            {
                From = new MailAddress(email)
            };
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient smtpClient = new(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true
            };

            try
            {
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }


        /// <summary>
        /// Get All Phone Call Notes of Customer
        /// </summary>
        /// <param name="input">Input should be Customer Id</param>
        /// <returns></returns>
        public async Task<ListResultDto<NoteListDto>> GetCustomerPhoneCallNotesAsync(EntityDto input)
        {
            ActivityType phoneCallActivityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                phoneCallActivityType = await _activityTypeRepository.GetAll().FirstOrDefaultAsync(a => a.Name == OpticianConsts.PhoneCallActivityType);
            }
            IQueryable<Note> query = _noteRepository.GetAll();
           // query = query.Where(note => note.CustomerId == input.Id && note.Activity.ActivityTypeId == phoneCallActivityType.Id);
             query = query.Where(note => note.UserId == input.Id && note.Activity.ActivityTypeId == phoneCallActivityType.Id);
            ListResultDto<NoteListDto> result = new(ObjectMapper.Map<List<NoteListDto>>(await query.ToListAsync()));
            return result;
        }

        /// <summary>
        /// Get All Sms Notes of Customer
        /// </summary>
        /// <param name="input">Input should be Customer Id</param>
        /// <returns></returns>
        public async Task<ListResultDto<NoteListDto>> GetCustomerSmsNotesAsync(EntityDto input)
        {
            ActivityType smsNoteActivityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                smsNoteActivityType = await _activityTypeRepository.GetAll().FirstOrDefaultAsync(at => at.Name == OpticianConsts.SmsNoteActivityType);
            }
            IQueryable<Note> query = _noteRepository.GetAll();
          //  query = query.Where(note => note.CustomerId == input.Id && note.Activity.ActivityTypeId == smsNoteActivityType.Id);
             query = query.Where(note => note.UserId == input.Id && note.Activity.ActivityTypeId == smsNoteActivityType.Id);
            ListResultDto<NoteListDto> result = new(ObjectMapper.Map<List<NoteListDto>>(await query.ToListAsync()));
            return result;
        }

        /// <summary>
        /// Get All Sms Notes of Customer
        /// </summary>
        /// <param name="input">Input should be Customer Id</param>
        /// <returns></returns>
        public async Task<ListResultDto<NoteListDto>> GetCustomerEmailNotesAsync(EntityDto input)
        {
            ActivityType emailNoteActivityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                emailNoteActivityType = await _activityTypeRepository.GetAll().FirstOrDefaultAsync(at => at.Name == OpticianConsts.EmailNoteActivityType);
            }
            IQueryable<Note> query = _noteRepository.GetAll();
            //query = query.Where(note => note.CustomerId == input.Id && note.Activity.ActivityTypeId == emailNoteActivityType.Id);
            query = query.Where(note => note.UserId == input.Id && note.Activity.ActivityTypeId == emailNoteActivityType.Id);
            ListResultDto<NoteListDto> result = new(ObjectMapper.Map<List<NoteListDto>>(await query.ToListAsync()));
            return result;
        }

        /// <summary>
        /// Get Default activity information for phone call note activity
        /// </summary>
        /// <returns></returns>
        public async Task<ActivityDto> GetPhoneCallNoteDefaultActivityAsync()
        {
            Activity activity = await GetDefaultActivityForNote(OpticianConsts.PhoneCallActivityType);
            return ObjectMapper.Map<ActivityDto>(activity);
        }


        /// <summary>
        /// Get Default activity information for SMS note activity
        /// </summary>
        /// <returns></returns>
        public async Task<ActivityDto> GetSMSNoteDefaultActivityAsync()
        {
            Activity activity = await GetDefaultActivityForNote(OpticianConsts.SmsNoteActivityType);
            return ObjectMapper.Map<ActivityDto>(activity);
        }

        /// <summary>
        /// Get Default activity information for Email note activity
        /// </summary>
        /// <returns></returns>
        public async Task<ActivityDto> GetEmailNoteDefaultActivityAsync()
        {
            Activity activity = await GetDefaultActivityForNote(OpticianConsts.EmailNoteActivityType);
            return ObjectMapper.Map<ActivityDto>(activity);
        }


        /// <summary>
        /// Get all booking activity types
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<ListResultDto<BookingActivityTypeDto>> GetAllBookingActivityTypesAsync()
        {
            using IDisposable unitOfWork = UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            IQueryable<BookingActivityType> query = _bookingActivityTypeRepository.GetAll();
            query = AbpSession.TenantId.HasValue
                ? query.Where(at => at.TenantId == null || at.TenantId == AbpSession.TenantId.Value)
                : query.Where(at => at.TenantId == null);
            List<BookingActivityTypeDto> list = MapBookingActivityTypeIntoDto(await query.ToListAsync());
            ListResultDto<BookingActivityTypeDto> result = new(list);
            return result;
        }
        /// <inheritdoc/>

        public static List<BookingActivityTypeDto> MapBookingActivityTypeIntoDto(List<BookingActivityType> bookingActivityTypes)
        {
            List<BookingActivityTypeDto> bookingActivityDtoList = new();
            foreach (BookingActivityType bookingActivityType in bookingActivityTypes)
            {
                bookingActivityDtoList.Add(new BookingActivityTypeDto()
                {
                    Id = bookingActivityType.Id,
                    Name = bookingActivityType.Name,
                    TimeInMinutes = bookingActivityType.TimeInMinutes,
                    Description = bookingActivityType.Description,
                    Duration = bookingActivityType.Duration,
                });

            }
            return bookingActivityDtoList;
        }

        #region Private Methods

        private async Task<Activity> GetDefaultActivityForNote(string activityTypeName)
        {
            long employeeId = AbpSession.UserId.Value;

            ActivityType activityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activityType = await _activityTypeRepository.
               FirstOrDefaultAsync(activityType => activityType.Name == activityTypeName);
            }
            ActivityArt activityArt = await _activityArtRepository
                .FirstOrDefaultAsync(activityArt => activityArt.Name == OpticianConsts.ActivityArtForPhoneCallActivity);

            ActivityType followUpType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                followUpType = await _activityTypeRepository.
               FirstOrDefaultAsync(activityType => activityType.Name == OpticianConsts.FollowUpActivityTypeForPhoneCallActivity);
            }
            Activity activity = Activity.Create(AbpSession.TenantId.Value,null, activityTypeName , null
                , DateTime.UtcNow, DateTime.UtcNow, activityType.Id, followUpType.Id,
                activityArt.Id, employeeId, 0, null);
            return activity;
        }

        private async Task<Activity> CreateNoteActivityAsync(NoteActivityInputDto input, string activityTypeName)
        {
            int tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            Activity activity = GetActivityModel(input, tenantId);
            return await _activityManager.CreateAsync(activity);
        }

        private async Task CreateNote(NoteActivityInputDto input, Activity activity)
        {
            Note phoneCallNote = Note.Create(AbpSession.TenantId.Value, input.CustomerTableId,input.Title, input.Description, activity.Id,DateTime.Now, input.TicketId);
            _ = await _noteRepository.InsertAsync(phoneCallNote);
            if (input.EmployeeIds != null)
            {
                //Add Activity Responsibles
                if (input.EmployeeIds != null)
                {
                    foreach (var employeeId in input.EmployeeIds)
                    {
                        var activityResponsible = ActivityResponsible.Create(activity.Id, employeeId, null);
                        await _activityManager.AddActivityResponsibleAsync(activityResponsible);
                    }
                }
                if (input.GroupIds != null)
                {
                    foreach (var groupId in input.GroupIds)
                    {
                        var activityResponsible = ActivityResponsible.Create(activity.Id, null, groupId);
                        await _activityManager.AddActivityResponsibleAsync(activityResponsible);
                    }
                }
            }
        }

        public async Task<List<ActivityResponsible>> GetActivityResposibleAsync(String activityId)
        {
            return _activityResponsibleRepository.GetAll()
                .Where(a => a.ActivityId == Convert.ToInt32(activityId))
                .ToList();
        }

        private static Activity GetActivityModel(CreateActivityDto activity, int tenantId)
        {
            return Activity.Create(tenantId,activity.FollowUpByEmployeeId, activity.Name,activity.GroupId, DateTime.ParseExact(activity.Date, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), DateTime.ParseExact(activity.FollowUpDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), activity.ActivityTypeId, activity.FollowUpTypeId, activity.ActivityArtId, activity.EmployeeId, activity.CustomerId, activity.RoomId, false);
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

        private static IQueryable<ActivityDto> GetSelectQueryForGetActivity(IQueryable<Activity> query)
        {
            return query.Select(a => new ActivityDto
            {
                Id = a.Id,
                Name = a.Name,
                CustomerId = a.Customer != null ? a.Customer.Customer.Id : null,
                ActivityTypeId = a.ActivityTypeId,
                ActivityArtId = a.ActivityArtId,
                Date = a.Date,
                FollowUpDate = a.FollowUpDate,
                FollowUpTypeId = a.FollowUpTypeId,
                CreationTime = a.CreationTime,
                CreatorUserId = a.CreatorUserId,
                EmployeeId = a.UserId,
                IsFollowUp = a.IsFollowUp,
                FollowUpByEmployeeId = a.FollowUpByEmployeeId,
                RoomId = a.RoomId,
                Note = a.Note.Description
            });
        }
        private IQueryable<Activity> ApplyDateRangeWithRoomFilter(GetActivitiesInputDto input, DateTime fromDate, DateTime toDate, IQueryable<Activity> query)
        {
            query = query.Where(a => a.Date.Date >= fromDate.Date && a.Date.Date <= toDate.Date);
            if (input.RoomId.HasValue)
            {
                query = query.Where(a => a.RoomId == input.RoomId);
            }

            int? tenantId = AbpSession.TenantId;
            if (tenantId.HasValue)
            {
                query = query.Where(a => a.TenantId == tenantId);
            }

            return query;
        }
        /// <inheritdoc/>

        #endregion

        #region CreateProductItemActivity
        public async Task CreateProductItemActivity(CreateProductItemActivityDto input)
        {

            long employeeId = AbpSession.UserId.Value;

            ActivityType activityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activityType = await _activityTypeRepository.
               FirstOrDefaultAsync(activityType => activityType.Name == OpticianConsts.ProductItemActivityType);
                activityType ??= await CreateNewActivityType(OpticianConsts.ProductItemActivityType);
            }

            ActivityType followUpActivityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                followUpActivityType = await _activityTypeRepository.
               FirstOrDefaultAsync(activityType => activityType.Name == OpticianConsts.ProductItemFollowUpActivityType);
                followUpActivityType ??= await CreateNewActivityType(OpticianConsts.ProductItemFollowUpActivityType);
            }
            ActivityArt activityArt = await _activityArtRepository
                .FirstOrDefaultAsync(activityArt => activityArt.Name == OpticianConsts.ActivityArtForPhoneCallActivity);

            Activity activity = Activity.Create(AbpSession.TenantId.Value,null, activityType.Name,null
                , DateTime.UtcNow, DateTime.UtcNow, activityType.Id, followUpActivityType.Id,
                activityArt.Id, employeeId, input.CustomerId, null);

            activity = await _activityManager.CreateAsync(activity);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            input.ActivityId = activity.Id;

            await _productItemAppService.UpdateProductItemInformation(input.ProductItemId, input.InvoiceId, input.InvoiceLineId, activity.Id, input.Note);
        }


        public async Task CreateFaultPhoneCallActivity(CreateProductItemActivityDto input)
        {

            long employeeId = AbpSession.UserId.Value;

            ActivityType activityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activityType = await _activityTypeRepository.
               FirstOrDefaultAsync(activityType => activityType.Name == OpticianConsts.FaultPhoneCallType);
                activityType ??= await CreateNewActivityType(OpticianConsts.FaultPhoneCallType);
            }

            ActivityType followUpActivityType;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                followUpActivityType = await _activityTypeRepository.
               FirstOrDefaultAsync(activityType => activityType.Name == OpticianConsts.ProductItemFollowUpActivityType);
                followUpActivityType ??= await CreateNewActivityType(OpticianConsts.ProductItemFollowUpActivityType);
            }
            ActivityArt activityArt = await _activityArtRepository
                .FirstOrDefaultAsync(activityArt => activityArt.Name == OpticianConsts.ActivityArtForPhoneCallActivity);

            Activity activity = Activity.Create(AbpSession.TenantId.Value,null, activityType.Name,null
                , DateTime.UtcNow, DateTime.UtcNow, activityType.Id, followUpActivityType.Id,
                activityArt.Id, employeeId, input.CustomerId, null);

            activity = await _activityManager.CreateAsync(activity);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            input.ActivityId = activity.Id;

        }

        private async Task<ActivityType> CreateNewActivityType(string productItemActivityType)
        {
            ActivityType activityType = ActivityType.Create(null, productItemActivityType, 0, 0);
            activityType = await _activityTypeRepository.InsertAsync(activityType);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return activityType;
        }
        /// <inheritdoc/>

        public async Task CreateCheckInActivityAndAddNote(NoteActivityInputDto input)
        {
            try
            {
                ActivityType activityTye;
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    activityTye = await _activityTypeRepository.GetAll().FirstOrDefaultAsync(a => a.Name == OpticianConsts.CheckInActivityType);
                }
                if (activityTye != null && activityTye.Id > 0)
                {
                    input.ActivityTypeId = activityTye.Id;
                    input.FollowUpTypeId = activityTye?.Id;
                }

                Activity activity = await CreateNoteActivityAsync(input, OpticianConsts.CheckInActivityType);
                await CurrentUnitOfWork.SaveChangesAsync();
                await CreateNote(input, activity);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the last Activity "Check In" or "Check Out"
        /// </summary>
        /// <param name="input"></param>
        /// <returns>NoteListDto or null if no record found</returns>
        public async Task CreateCheckOutActivityAndAddNote(NoteActivityInputDto input)
        {
            ActivityType activityTYpe;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activityTYpe = await _activityTypeRepository.GetAll().FirstOrDefaultAsync(a => a.Name == OpticianConsts.CheckOutActivityType);
            }
            if (activityTYpe != null && activityTYpe.Id > 0)
            {
                input.ActivityTypeId = activityTYpe.Id;
                input.FollowUpTypeId = activityTYpe?.Id;

            }

            Activity activity = await CreateNoteActivityAsync(input, OpticianConsts.CheckOutActivityType);
            await CurrentUnitOfWork.SaveChangesAsync();
            await CreateNote(input, activity);
        }
        /// <inheritdoc/>


        public async Task<NoteListDto> GetLastCheckInCheckOutNotesAsync(EntityDto input)
        {
            try
            {
                List<ActivityType> activityTYpe;
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    activityTYpe = await _activityTypeRepository.GetAllListAsync(a => a.Name == OpticianConsts.CheckInActivityType || a.Name == OpticianConsts.CheckOutActivityType);
                }
                List<int> activityTypeIds = activityTYpe.Select(activityType => activityType.Id).ToList();
                Note latestNote = await _noteRepository.GetAllIncluding(note => note.Activity)
                   .Where(note => note.CreatorUserId == input.Id && activityTypeIds.Contains(note.Activity.ActivityTypeId))
                   .OrderByDescending(note => note.CreationTime)
                   .FirstOrDefaultAsync();
                if (latestNote == null) { return null; };
                NoteListDto result = new()
                {
                    Id = latestNote.Id,
                   // CustomerId = latestNote.CustomerId,
                    UserId = latestNote.UserId,
                    Description = latestNote.Description,
                    CreationTime = latestNote.CreationTime,
                    ActivityName = latestNote.Activity.Name,
                    ActivityTypeId = latestNote.Activity.ActivityTypeId
                };
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <inheritdoc/>

        public async Task<ListResultDto<NoteListDto>> GetCheckOutNotesAsync(EntityDto input)
        {
            ActivityType activityTYpe;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activityTYpe = await _activityTypeRepository.GetAll().FirstOrDefaultAsync(a => a.Name == OpticianConsts.CheckOutActivityType);
            }
            IQueryable<Note> query = _noteRepository.GetAll();
            query = query.Where(note => note.CreatorUserId == input.Id && note.Activity.ActivityTypeId == activityTYpe.Id);
            ListResultDto<NoteListDto> result = new(ObjectMapper.Map<List<NoteListDto>>(await query.ToListAsync()));
            return result;
        }
        /// <inheritdoc/>

        public async Task<ListResultDto<NoteListDto>> GetCheckInNotesAsync(EntityDto input)
        {
            ActivityType activity;
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activity = await _activityTypeRepository.GetAll().FirstOrDefaultAsync(a => a.Name == OpticianConsts.CheckInActivityType);
            }
            IQueryable<Note> query = _noteRepository.GetAll();
            query = query.Where(note => note.CreatorUserId == input.Id && note.Activity.ActivityTypeId == activity.Id);
            ListResultDto<NoteListDto> result = new(ObjectMapper.Map<List<NoteListDto>>(await query.ToListAsync()));
            return result;
        }


        #endregion
    }
}
