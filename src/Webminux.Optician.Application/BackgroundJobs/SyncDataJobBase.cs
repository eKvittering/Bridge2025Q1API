using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using Webminux.Optician.Application.Activities.Dto;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Companies;
using Webminux.Optician.Core;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.Invoices;
using Webminux.Optician.Helpers;
namespace Webminux.Optician.BackgroundJobs
{
    public abstract class SyncDataJobBase : ITransientDependency
    {
        internal readonly IRepository<User, long> _userRepository;
        internal readonly IRepository<EconomicSyncHistory, int> _economicSyncHistoryRepository;
        internal readonly IRepository<SyncHistoryDetail, int> _syncHistoryDetailRepository;
        internal readonly IRepository<Customer, int> _customerRepository;
        internal readonly IRepository<Invoice, int> _invoiceRepository;
        internal readonly IRepository<UserType, int> _userTypeRepository;
        internal readonly IRepository<InvoiceLine, int> _invoiceLineRepository;
        internal string _BaseUrl = "";
        internal readonly IUnitOfWorkManager _unitOfWorkManager;
        internal readonly IObjectMapper _objectMapper;
        internal readonly UserManager _userManager;
        internal int _defaulPageSize = 1000;
        internal int userTypeId;
        internal readonly IAbpSession _session;
        internal readonly IRepository<ActivityArt> _activityArtRepository;
        internal readonly IRepository<ActivityType> _activityTypeRepository;
        internal readonly IRepository<Company> _companyRepository;
        internal readonly IActivityManager _activityManager;
        internal readonly IRepository<Product, int> _productRepository;
        internal readonly IRepository<ProductGroup, int> _productGroupRepository;

        public SyncDataJobBase(
            IRepository<User, long> userRepository,
            IRepository<EconomicSyncHistory, int> economicSyncHistoryRepository,
            IRepository<SyncHistoryDetail, int> syncHistoryDetailRepository,
            IRepository<Customer, int> customerRepository,
            IRepository<Invoice, int> invoiceRepository,
            IRepository<UserType, int> userTypeRepository,
            IRepository<InvoiceLine, int> invoiceLineRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IObjectMapper objectMapper,
            UserManager userManager,
            IAbpSession session,
            IRepository<ActivityArt> activityArtRepository,
            IRepository<ActivityType> activityTypeRepository,
            IRepository<Company> companyRepository,
            IActivityManager activityManager,
            IRepository<Product, int> productRepository,
            IRepository<ProductGroup, int> productGroupRepository)
        {
            _userRepository = userRepository;
            _economicSyncHistoryRepository = economicSyncHistoryRepository;
            _syncHistoryDetailRepository = syncHistoryDetailRepository;
            _customerRepository = customerRepository;
            _invoiceRepository = invoiceRepository;
            _userTypeRepository = userTypeRepository;
            _invoiceLineRepository = invoiceLineRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _objectMapper = objectMapper;
            _userManager = userManager;
            _session = session;
            _activityArtRepository = activityArtRepository;
            _activityTypeRepository = activityTypeRepository;
            _companyRepository = companyRepository;
            _activityManager = activityManager;
            _productRepository = productRepository;
            _productGroupRepository = productGroupRepository;
        }
        public abstract Task Execute(DataImportJobInputDto args);
        internal async Task CreateSaleActivityWhileSyncInvoice(long customerId, string invoiceNo, int tenantId)
        {
            CreateActivityDto createActivityDto = new CreateActivityDto();


            var activityTypes = await _activityTypeRepository.GetAll().GetLookupResultAsync<ActivityType, int>();
            var activityArtType = await _activityArtRepository.GetAll().GetLookupResultAsync<ActivityArt, int>();

            var userTypeId = _userTypeRepository.GetAll().Where(x => x.Name == OpticianConsts.UserTypes.Employee).FirstOrDefault().Id;

            createActivityDto.CustomerId = customerId;
            createActivityDto.Name = "Invoice " + "" + invoiceNo;

            createActivityDto.EmployeeId = _userRepository.GetAll().Where(x => x.UserTypeId == userTypeId && x.TenantId == tenantId).FirstOrDefault().Id;
            createActivityDto.ActivityArtId = activityArtType.Items.Where(x => x.Name == OpticianConsts.ActivityArts.Activity).FirstOrDefault().Id;
            createActivityDto.ActivityTypeId = activityTypes.Items.Where(x => x.Name == OpticianConsts.ActivityTypes.Sale).FirstOrDefault().Id;
            createActivityDto.FollowUpTypeId = activityTypes.Items.Where(x => x.Name == OpticianConsts.ActivityTypes.Sale).FirstOrDefault().Id;
            createActivityDto.Date = DateTime.UtcNow.ToString("yyyy-MM-dd");
            createActivityDto.FollowUpDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            await _activityManager.CreateAsync(GetActivityModel(createActivityDto, tenantId));
        }
        private static Activity GetActivityModel(CreateActivityDto activity, int tenantId)
        {
            return Activity.Create(tenantId,activity.FollowUpByEmployeeId, activity.Name,activity.GroupId, DateTime.ParseExact(activity.Date, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), DateTime.ParseExact(activity.FollowUpDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), activity.ActivityTypeId, activity.FollowUpTypeId, activity.ActivityArtId, activity.EmployeeId, activity.CustomerId,null);
        }
    }
}
