using System.Runtime.Serialization;
using Webminux.Optician.Debugging;

namespace Webminux.Optician
{
    public class OpticianConsts
    {
        public const string LocalizationSourceName = "Optician";

        public const string ConnectionStringName = "Default";
        public const string CloudinarySettingsKey = "CloudinarySettings";
        public const string DateFormate = "yyyy-MM-dd";
        public const string DateTimeFormate = "yyyy-MM-dd HH:mm:ss";
        public const bool MultiTenancyEnabled = true;
        public const int MaxTitleLength = 128;
        public const int MaxDescriptionLength = 2048;
        public const int DefaultTenantId = 1;
        public const string SmsNoteActivityType = "SMS Note";
        public const string EmailNoteActivityType = "Email Note";
        public const string PhoneCallActivityType = "Phone Call Note";
        public const string ProductItemActivityType = "Eilepsy Sale";
        public const string FaultPhoneCallType = "Fault Phone Call";
        public const string ProductItemFollowUpActivityType = "Medical Device Review";
        public const string ProductItemActivityArt = "Activity";
        public const string FollowUpActivityTypeForPhoneCallActivity = "Remember Write/call (R18)";
        public const string CheckInActivityType = "Check In";
        public const string CheckOutActivityType = "Check Out";
        public const string DefautGroupName = "Care Center";

        public const string ActivityArtForPhoneCallActivity = "Activity";
        public const string DefaultAdminIdSettingKey = "DefaultAdminId";
        public const string DefaultTenantIdSettingKey = "DefaultTenantId";
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "0247c1daad214d8b810c8d4595ff2507";

        public const string DefaultItemCode = "003-132";
        public static class ActivityArts
        {
            public const string LetterSend = "Letter Send";
            public const string Activity = "Activity";
            public const string CustomerResponse = "Customer Response";
            public const string MachineActivity = "Machine Activity";
        }


        public static class ActivityTypes
        {
            public const string Sale = "Sale (A3)";
        }

        public static class ErrorMessages
        {
            public const string ActivityNotFound = "Activity not found";
            public const string GroupNotFound = "Group not found";
            public const string NotFound = "Not found";
            public const string InviteNotFound = "Invite not found";
            public const string TaskNotFound = "Task not found";
            public const string CategoryNotFound = "Category not found";
            public const string BrandNotFound = "Brand not found";
            public const string MenuItemNotFound = "Menu Item not found";
            public const string PageConfigNotFound = "Page Config not found";

            public const string MenuItemsNotFound = "Menu Items not found";
        }
        public static class UserTypes
        {
            public const string Employee = "Employee";
            public const string Customer = "Customer";
            public const string Supplier = "Supplier";
        }

        public static class MessageType
        {
            public const string Received = "received";
            public const string Sent = "sent";
        }

        public static class BookingConstants
        {
            public const string ActivityType = "Customer Booking";
        }



        public const string PackageRecieveActivity = "Recieve Package";
        public static class FalutConstants
        {
            public const string ActivityType = "Repair Item";
            public const string ActivityType2 = "Fault";
        }

        public static class TicketConstants
        {
            public const string ActivityType = "Ticket";
        }

        public static class InvoiceLineStatuses
        {
            public const string Draft = "Draft";
            public const string AwaitingSerialNumber = "Awaiting Serial Number";
            public const string Completed = "Completed";
        }
        public enum InviteResponse
        {
            Declined = 0,
            Accepted = 1,
            Pending = 2
        }
        public enum SyncApis
        {
            Economic = 1,
            Billy = 2
        }

        public enum CustomFieldType
        {
            Numeric = 1,
            String = 2
        }

        public enum Screen
        {
            Customer = 1,
            Product = 2
        }

        public enum FaultStatus
        {
            Open = 1,
            Resolved = 2
        }

        public enum TicketStatus
        {
            Open = 1,
            RMA = 2,
            Resolved = 3,
        }

        public enum EmployeeTicketStatus
        {
            Pending = 0,
            Accepted = 1,
            Rejected = 2
        }

        public enum BookingStatus
        {
            Open = 1,
            Close = 2
        }

        public enum BookingEmployeeStatus
        {
            Pending = 1,
            Accepted = 2,
            NotAllowed = 3,
            Rejected = 4
        }
    }
}
