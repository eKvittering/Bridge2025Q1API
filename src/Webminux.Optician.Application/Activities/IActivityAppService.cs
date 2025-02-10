using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Activities.Dto;
using Webminux.Optician.Application;
using Webminux.Optician.Application.Activities.Dto;

namespace Webminux.Optician
{
    /// <summary>
    /// Provides methods to Add, Update, Delete and Get Activity
    /// </summary>
    public interface IActivityAppService : IApplicationService
    {

        /// <summary>
        /// Create a new Activity
        /// </summary>
        Task CreateAsync(CreateActivityDto activity);

        /// <summary>
        /// Update an existing Activity
        /// </summary>
        Task UpdateAsync(UpdateActivityDto activity);

        /// <summary>
        /// Delete an existing Activity
        /// </summary>
        Task DeleteAsync(EntityDto id);

        /// <summary>
        /// Get an existing Activity
        /// </summary>
        Task<ActivityDto> GetAsync(EntityDto id);

        /// <summary>
        /// Get all Activities
        /// </summary>
        Task<ListResultDto<LookUpDto<int>>> GetAllAsync();

        /// <summary>
        /// Get Activities by page
        /// </summary>
        Task<PagedResultDto<ActivityListDto>> GetPagedResultAsync(PagedActivityRequestResultDto input);

        /// <summary>
        /// Get all ActivityTypes
        /// </summary>
        Task<ListResultDto<LookUpDto<int>>> GetAllActivityTypesAsync();

        /// <summary>
        /// Get all ActivityArts
        /// </summary>
        Task<ListResultDto<LookUpDto<int>>> GetAllActivityArtsAsync();

        /// <summary>
        /// Get all Activities by date filter
        /// </summary>
        Task<ListResultDto<ActivityListDto>> GetAllActivitiesAsync(GetActivitiesInputDto input);

        /// <summary>
        /// Mark an existing Activity as done
        /// </summary>
        Task MarkActivityAsFollowUpAsync(EntityDto input);

        /// <summary>
        /// Create Phone call activity and add note to customer
        /// </summary>
        Task CreatePhoneCallNoteActivityAndAddNote(NoteActivityInputDto input);


        /// <summary>
        /// Create Phone call activity and add note to customer
        /// </summary>
        Task CreateCheckInActivityAndAddNote(NoteActivityInputDto input);


        /// <summary>
        /// 
        /// </summary>
        Task CreateCheckOutActivityAndAddNote(NoteActivityInputDto input);


        /// <summary>
        /// Get List of Customer Notes
        /// </summary>
        Task<ListResultDto<NoteListDto>> GetCheckOutNotesAsync(EntityDto input);




        /// <summary>
        /// Get List of Customer Notes
        /// </summary>
        Task<ListResultDto<NoteListDto>> GetCheckInNotesAsync(EntityDto input);

        Task<NoteListDto> GetLastCheckInCheckOutNotesAsync(EntityDto input);


        /// <summary>
        /// Get List of Customer Notes
        /// </summary>
        Task<ListResultDto<NoteListDto>> GetCustomerPhoneCallNotesAsync(EntityDto input);

        /// <summary>
        /// Get Default activity information for phone call note activity
        /// </summary>
        /// <returns></returns>
        Task<ActivityDto> GetPhoneCallNoteDefaultActivityAsync();

        /// <summary>
        /// Get Default activity information for SMS note activity
        /// </summary>
        /// <returns></returns>
        Task<ActivityDto> GetSMSNoteDefaultActivityAsync();

        /// <summary>
        /// Get Default activity information for Email note activity
        /// </summary>
        /// <returns></returns>
        Task<ActivityDto> GetEmailNoteDefaultActivityAsync();

        /// <summary>
        /// Get all booking activity types
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<BookingActivityTypeDto>> GetAllBookingActivityTypesAsync();
        Task CreateProductItemActivity(CreateProductItemActivityDto input);

        //void SendMail(string to, string subject, string body);
        Task SendMessageAsync(string message, string phoneNumber);
        Task CreateFaultPhoneCallActivity(CreateProductItemActivityDto input);
 
    }
}
