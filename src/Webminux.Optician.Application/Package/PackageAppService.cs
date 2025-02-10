using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician;
using Webminux.Optician.Activities;
using Webminux.Optician.Application.Activities.Dto;
using Webminux.Optician.Core.Notes;
using Webminux.Optician.PackageType;
using Webminux.Optician.SubPackage;
using Webminux.Optician.Users;
using Webminux.Optician.Users.Dto;
using static Webminux.Optician.OpticianConsts;


/// <summary>
/// Provides methods to manage Groups
/// </summary>
public class PackageAppService : OpticianAppServiceBase, IPackageService
{
    private readonly IRepository<Webminux.Optician.Package.Pacakge, int> _repository;
    private readonly IRepository<Webminux.Optician.SubPackage.SubPackage, int> _subPackageRepository;
    private readonly IMediaHelperService _imageHelperService;
    private readonly IActivityManager _activityManager;
    private readonly IRepository<ActivityType> _activityTypeRepository;
    private readonly IRepository<ActivityArt> _activityArtRepository;
    private readonly IRepository<PackageType> _packageRepository;
    private readonly IUserAppService _userAppService;
    private readonly IActivityAppService _activityService;
    private readonly IRepository<Webminux.Optician.Activity> _activityRepository;
    private readonly IRepository<Note> _noteRepository;
   



    /// <summary>
    /// Constructor
    /// </summary>
    public PackageAppService(
        IRepository<Webminux.Optician.Package.Pacakge, int> repository,
        IRepository<Webminux.Optician.SubPackage.SubPackage, int> subPackageRepository,
        IMediaHelperService imageHelperService,
        IActivityManager activityManager,
        IRepository<ActivityType> activityTypeRepository,
        IRepository<ActivityArt> activityArtRepository,
        IRepository<PackageType> packageRepository,
        IUserAppService userAppService,
       // IActivityAppService activityRepository,
        IRepository<Note> noteRepository,
        IRepository<Webminux.Optician.Activity> activityRepository,
        IActivityAppService activityService

        )
    {
        _repository = repository;
        _imageHelperService = imageHelperService;
        _subPackageRepository = subPackageRepository;
        _activityManager = activityManager;
        _activityTypeRepository = activityTypeRepository;
        _activityArtRepository = activityArtRepository;
        _packageRepository = packageRepository;
        _userAppService = userAppService;
        _noteRepository = noteRepository;
        _activityService = activityService;
        _activityRepository = activityRepository;



    }

    /// <summary>
    /// Create a new Package
    /// </summary>
    public async Task CreateAsync(CreatePackageDto input)
    {

       
        try
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            

            MediaUploadDto uploadResult = new MediaUploadDto();
            if (!string.IsNullOrWhiteSpace(input.Base64Picture))
                uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);

            if (input.SenderId > 0)
            {
                //if(input.FollowUpTypeId > 0)
                //{
                    var activity = await CreatePackageActivity((long)input.SenderId, tenantId, input.EmployeeId, input.FollowUpTypeId, DateTime.ParseExact(input.FollowUpDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), input.FllowUpEmployeeId, input.FllowUpGroupId);
                    input.ActivityId = activity.Id;
                    Note phoneCallNote = Note.Create(AbpSession.TenantId.Value, (int)input.SenderId, input.ActivityNote, input.ActivityNote, activity.Id, DateTime.Now, null);
                    await _noteRepository.InsertAsync(phoneCallNote);


                    if (input.FllowUpEmployeeId > 0)
                    {
                        var activityResponsible = ActivityResponsible.Create(activity.Id, input.EmployeeId, null);
                        await _activityManager.AddActivityResponsibleAsync(activityResponsible);

                    }
                    if (input.FllowUpGroupId > 0)
                    {

                        var activityResponsible = ActivityResponsible.Create(activity.Id, null, input.FllowUpGroupId);
                        await _activityManager.AddActivityResponsibleAsync(activityResponsible);

                    }

                //}

            }

            var Package = Webminux.Optician.Package.Pacakge.Create(input.ActivityId,tenantId, input.EmployeeId, DateTime.ParseExact(input.PackageReceiveDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), input.PackageTypeId, uploadResult.PublicId, uploadResult.Url, input.SenderId, input.Description, input.OuterSenderFirstName, input.OuterSenderLastName, input.OuterSenderEmail, input.OuterSenderPhoneNumber );
            await _repository.InsertAsync(Package);
            UnitOfWorkManager.Current.SaveChanges();

            if (input.CreateSubPackageDtos != null && input.CreateSubPackageDtos.Count() > 0)
            {
                foreach (var item in input.CreateSubPackageDtos)
                {

                    item.PackageId = Package.Id;
                    await CreateSubPackageAsync(item);
                }
            }

         



        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    private async Task<Webminux.Optician.Activity> CreatePackageActivity(long userId, int tenantId, int responsibleEmployee, int? followTyeId ,DateTime FollowUpDate, int? FllowUpEmployeeId,int? FllowUpGroupId)
    {
        ActivityType activityType;
        using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
        {
            activityType = await _activityTypeRepository.FirstOrDefaultAsync(activityType => activityType.Name == PackageRecieveActivity);
            if (activityType == null)
            {
                activityType = await CreateNewActivityType(PackageRecieveActivity);
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
        //var activity = Webminux.Optician.Activity.Create(tenantId,(long)FllowUpEmployeeId,activityType.Name, FllowUpGroupId, currentDate, currentDate, activityType.Id, followTyeId, (int)activityArt.Id, responsibleEmployee, userId,null ,false);
        var activity = Webminux.Optician.Activity.Create(
                       tenantId,
                       FllowUpEmployeeId.HasValue ? FllowUpEmployeeId.Value : (long?)null,
                       activityType?.Name,
                       FllowUpGroupId > 0 ? FllowUpGroupId : (int?)null,
                       currentDate,
                       FollowUpDate,
                       activityType?.Id ?? 0,
                       followTyeId > 0 ? followTyeId : (int?)null,
                       activityArt?.Id ?? 0,
                       responsibleEmployee > 0 ? responsibleEmployee : (long?)null,
                       userId > 0 ? userId : (long?)null,
                       null,
                       false
                      );


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

    private async Task CreateSubPackageAsync(CreateSubPackageDto input)
    {

        try
        {

            MediaUploadDto uploadResult = new MediaUploadDto();
            if (!string.IsNullOrWhiteSpace(input.Base64Picture))
                uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);


            var SubPackage = Webminux.Optician.SubPackage.SubPackage.Create(input.PackageId, input.Contains, uploadResult.PublicId, uploadResult.Url);
            await _subPackageRepository.InsertAsync(SubPackage);
            UnitOfWorkManager.Current.SaveChanges();

        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    //private async Task updateSubPackageAsync(CreateSubPackageDto input)
    //{
    //    try
    //    {
    //        // Step 1: Retrieve the existing subpackage using the PackageId (or another identifier)
    //        var subPackage = await _subPackageRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

    //        if (subPackage == null)
    //        {
    //            throw new Exception("SubPackage not found.");
    //        }

    //        // Step 2: Update the properties of the subpackage
    //        subPackage.Contains = input.Contains;

    //        //// Step 3: Check if a new Base64Picture is provided
    //        //if (!string.IsNullOrWhiteSpace(input.Base64Picture))
    //        //{
    //        //    // Upload the new image and get the new public ID and URL
    //        //    var uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);

    //        //    // Update the subpackage's image details
    //        //    subPackage.ImagePublicKey = uploadResult.PublicId;
    //        //    subPackage.ImageUrl = uploadResult.Url;
    //        //}

    //        // Step 4: Update the subpackage in the repository
    //        await _subPackageRepository.UpdateAsync(subPackage);

    //        // Step 5: Commit the transaction to save changes
    //        await UnitOfWorkManager.Current.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the error (optional) and rethrow the exception to handle it at a higher level
    //        // _logger.LogError(ex, "Error while updating subpackage");
    //        throw;
    //    }
    //}

    //private async Task deleteSubPackageAsync()
    //{
    //    try
    //    {

    //    }
    //     catch (Exception ex)
    //    {
    //        throw new Exception($"Error while delete subpackage: {ex.Message}", ex);
    //    }
    //}

    //private async Task updateSubPackageAsync(CreateSubPackageDto input)
    //{
    //    try
    //    {
    //        // Step 1: Retrieve the existing subpackage using the PackageId (or another identifier)
    //        var subPackage = await _subPackageRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

    //        if (subPackage == null)
    //        {
    //            throw new Exception("SubPackage not found.");
    //        }

    //        // Step 2: Update the properties of the subpackage
    //        subPackage.Contains = input.Contains;

    //        // Step 3: Check if a new Base64Picture is provided
    //        if (!string.IsNullOrWhiteSpace(input.Base64Picture))
    //        {
    //            // Upload the new image and get the new public ID and URL
    //            var uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);

    //            if (uploadResult != null)
    //            {
    //                // Update the subpackage's image details
    //                subPackage.ImagePublicKey = uploadResult.PublicId;
    //                subPackage.ImageUrl = uploadResult.Url;
    //            }
    //        }

    //        // Step 4: Update the subpackage in the repository
    //        _subPackageRepository.Update(subPackage);

    //        // Step 5: Commit the transaction to save changes
    //        await UnitOfWorkManager.Current.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the error (optional) and rethrow the exception to handle it at a higher level
    //        //_logger.LogError(ex, "Error while updating subpackage");
    //        throw new Exception($"Error while updating subpackage: {ex.Message}", ex);
    //    }
    //}
    private async Task updateSubPackageAsync(CreateSubPackageDto input)
    {
        try
        {
            // Step 1: Retrieve the existing subpackage
            var subPackage = await _subPackageRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (subPackage == null)
            {
                throw new Exception("SubPackage not found.");
            }

            // Step 2: Update properties
            subPackage.Contains = input.Contains;

            //// Step 3: Handle Base64Picture

            if (!string.IsNullOrWhiteSpace(input.Base64Picture) && input.Base64Picture.StartsWith("data:image"))
            {
                // Upload the new image
                var uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);
                if (uploadResult != null)
                {
                    subPackage.ImagePublicKey = uploadResult.PublicId;
                    subPackage.ImageUrl = uploadResult.Url;
                }
            }

            // Step 4: Update the repository
            _subPackageRepository.Update(subPackage);

            // Step 5: Commit the transaction
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while updating subpackage: {ex.Message}", ex);
        }
    }


   


    /// <summary>
    /// Get a Package
    /// </summary>
    public async Task<PackageDto> GetByIdAsync(int id)
    {
        UserDto sentUserInfo = new UserDto();
        // Fetch the package by ID
        var package = await _repository.FirstOrDefaultAsync(b => b.Id == id);
        if (package == null)
        {
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);
        }

        // Fetch related data
        if (package.SenderId > 0)
        {
            sentUserInfo = await _userAppService.GetByUserId((long)package.SenderId);
        }

        var packageType = await _packageRepository.FirstOrDefaultAsync(b => b.Id == package.PackageTypeId && package.PackageTypeId > 0);
        var note = await _noteRepository.FirstOrDefaultAsync(n => n.ActivityId == package.ActivityId);
        int activityId = (int)(package.ActivityId ?? 0);      

        if (activityId > 0)
        {
            var  activityResponse = await _activityManager.GetAsync(activityId);
            var subPackagesForPackage = await _subPackageRepository.GetAllListAsync();
            var subPackageDtos = subPackagesForPackage
                .Where(p => p.PackageId == package.Id)
                .Select(p => new SubPackageDto
                {

                    Id = p.Id,
                    PackageId = p.PackageId,
                    Contains = p.Contains,
                    CreationTime = p.CreationTime,
                    CreatorUserId = p.CreatorUserId,
                    ImageUrl = p.ImageUrl,

                }).ToList();

            // Create the PackageDto
            var packageDetail = new PackageDto
            {
                Id = package.Id,
                CreationTime = package.CreationTime,
                CreatorUserId = package.CreatorUserId,
                EmployeeId = package.EmployeeId,
                ImageUrl = package.ImageUrl,
                PackageTypeId = package.PackageTypeId,
                SenderId = package.SenderId,
                SenderFullName = sentUserInfo != null ? $"{sentUserInfo.FullName}" : string.Empty,
                SenderEmail = sentUserInfo?.EmailAddress,
                SenderUserTypeId = sentUserInfo?.UserTypeId ?? 0,
                PackageTypeName = packageType?.Name,
                SubPackageDtos = subPackageDtos,
                ReceiveDate = package.Date,
                Description = package.Description,

                OuterSenderFirstName = package.OuterSenderFirstName,
                OuterSenderLastName = package.OuterSenderLastName,
                OuterSenderPhoneNumber = package.OuterSenderPhoneNumber,
                OuterSenderEmail = package.OuterSenderEmail,
                NoteDescription = note.Description,
                ActivityId = activityResponse?.Id ?? 0,
                FollowUpDate = activityResponse.FollowUpDate != null ? activityResponse.FollowUpDate.ToString("yyyy-MM-dd") : string.Empty,
                ActivityTypeId = activityResponse.ActivityTypeId,
                FollowUpTypeId = activityResponse.FollowUpTypeId,
                FllowUpEmployeeId = activityResponse.FollowUpByEmployeeId,
                FllowUpGroupId = activityResponse.GroupId,


            };
            return packageDetail;
        }
        else
        {
            var subPackagesForPackage = await _subPackageRepository.GetAllListAsync();
            var subPackageDtos = subPackagesForPackage
                .Where(p => p.PackageId == package.Id)
                .Select(p => new SubPackageDto
                {

                    Id = p.Id,
                    PackageId = p.PackageId,
                    Contains = p.Contains,
                    CreationTime = p.CreationTime,
                    CreatorUserId = p.CreatorUserId,
                    ImageUrl = p.ImageUrl,

                }).ToList();

            // Create the PackageDto
            var packageDetail = new PackageDto
            {
                Id = package.Id,
                CreationTime = package.CreationTime,
                CreatorUserId = package.CreatorUserId,
                EmployeeId = package.EmployeeId,
                ImageUrl = package.ImageUrl,
                PackageTypeId = package.PackageTypeId,
                SenderId = package.SenderId,
                SenderFullName = sentUserInfo != null ? $"{sentUserInfo.FullName}" : string.Empty,
                SenderEmail = sentUserInfo?.EmailAddress,
                SenderUserTypeId = sentUserInfo?.UserTypeId ?? 0,
                PackageTypeName = packageType?.Name,
                SubPackageDtos = subPackageDtos,
                ReceiveDate = package.Date,
                Description = package.Description,

                OuterSenderFirstName = package.OuterSenderFirstName,
                OuterSenderLastName = package.OuterSenderLastName,
                OuterSenderPhoneNumber = package.OuterSenderPhoneNumber,
                OuterSenderEmail = package.OuterSenderEmail,
                        

            };
            return packageDetail;
        }      
       

    }




    /// <summary>
    /// Get a Package
    /// </summary>
    public async Task<List<PackageDto>> GetListAsync()
    {
        var categories = await getPackageDtoList();
        return categories.ToList();

    }


    /// <summary>
    /// Update a Catgory
    /// </summary>
    public async Task UpdateAsync(UpdatePackageDto input)
    {
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
        try
        {
            if (input.Id > 0)
            {
                var noteData =  _noteRepository.FirstOrDefault(x=>x.ActivityId == input.ActivityId);
                var activityData = _activityRepository.FirstOrDefault(x => x.Id == input.ActivityId);

                // Retrieve the package data
                var data = await _repository.GetAsync(input.Id);
                if (data == null)
                    throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);

                // Handle image upload
                MediaUploadDto uploadResult = new MediaUploadDto();
                if (!string.IsNullOrWhiteSpace(input.Base64Picture) && input.Base64Picture.StartsWith("data:image"))
                {
                    uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);
                    data.ImageUrl = uploadResult.Url;
                    data.ImagePublicKey = uploadResult.PublicId;
                }

                // Update package fields
                data.Description = input.Description;
                data.EmployeeId = input.EmployeeId;
                data.PackageTypeId = input.PackageTypeId;
                data.SenderId = input.SenderId;
                //data.SenderType = input.SenderType;
                data.OuterSenderFirstName = input.OuterSenderFirstName;
                data.OuterSenderLastName = input.OuterSenderLastName;
                data.OuterSenderEmail = input.OuterSenderEmail;
                data.OuterSenderPhoneNumber = input.OuterSenderPhoneNumber;
                //data.FollowUpDate = input.FollowUpDate
                data.Date = DateTime.ParseExact(input.PackageReceiveDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture);

                //if (input.SenderId > 0)
                //{
                //    var activity = await CreatePackageActivity((long)input.SenderId, tenantId, input.EmployeeId, input.FollowUpTypeId, input.FllowUpEmployeeId, input.FllowUpGroupId);

                //    Note phoneCallNote = Note.Create(AbpSession.TenantId.Value, (int)input.SenderId, input.ActivityNote, input.ActivityNote, activity.Id, DateTime.Now, null);
                //    await _noteRepository.InsertAsync(phoneCallNote);
                //    if (input.FllowUpEmployeeId > 0)
                //    {
                //        var activityResponsible = ActivityResponsible.Create(activity.Id, input.EmployeeId, null);
                //        await _activityManager.AddActivityResponsibleAsync(activityResponsible);

                //    }
                //    if (input.FllowUpGroupId > 0)
                //    {

                //        var activityResponsible = ActivityResponsible.Create(activity.Id, null, input.FllowUpGroupId);
                //        await _activityManager.AddActivityResponsibleAsync(activityResponsible);

                //    }

                //}

                if(noteData != null)
                {
                    noteData.Description = input.ActivityNote;
                    await _noteRepository.UpdateAsync(noteData);
                }

                if(activityData != null)
                {
                    activityData.ActivityTypeId = (int)input.ActivityTypeId;
                    activityData.FollowUpTypeId = input.FollowUpTypeId;
                    activityData.FollowUpDate = DateTime.Parse(input.FollowUpDate);
                    activityData.FollowUpByEmployeeId = input.FllowUpEmployeeId;
                    activityData.GroupId = input.FllowUpGroupId;                  

                    await _activityRepository.UpdateAsync(activityData);
                }

                await _repository.UpdateAsync(data);

                // Save the package changes
                await UnitOfWorkManager.Current.SaveChangesAsync();

               
                    
                    var existingSubPackages = await _subPackageRepository.GetAllListAsync(x => x.PackageId == input.Id);
                    
                    var inputSubPackageIds = input.CreateSubPackageDtos.Where(x => x.Id > 0).Select(x => x.Id).ToList();
                    
                    var subPackagesToDelete = existingSubPackages
                        .Where(x => !inputSubPackageIds.Contains(x.Id))
                        .ToList();

                    // Delete subpackages not in the input list
                    foreach (var subPackage in subPackagesToDelete)
                    {
                        await _subPackageRepository.DeleteAsync(subPackage);
                    }

                    // Update or create subpackages
                    foreach (var item in input.CreateSubPackageDtos)
                    {
                        if (item.Id > 0)
                        {                            
                            await updateSubPackageAsync(item);
                        }
                        else
                        {                            
                            await CreateSubPackageAsync(item);
                        }
                    }
               
            }
        }
        catch (Exception ex)
        {
            // Log and rethrow exception
            throw new Exception($"Error while updating package: {ex.Message}", ex);
        }
    }


    /// <summary>
    /// Delete a Package
    /// </summary>
    public async Task DeleteAsync(EntityDto input)
    {
        var PackageFromDB = await _repository.GetAsync(input.Id);
        if (PackageFromDB == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);

        await _repository.DeleteAsync(PackageFromDB);

    }

    /// <summary>
    /// Get all Package
    /// </summary>
    public async Task<ListResultDto<PackageDto>> GetAllAsync()
    {
        List<PackageDto> catgories = await getPackageDtoList();

        return new ListResultDto<PackageDto>(catgories);
    }

    private async Task<List<PackageDto>> getPackageDtoList()
    {
        return await (
            from cate in _repository.GetAll()
            select new PackageDto
            {
                Id = cate.Id,
                CreationTime = cate.CreationTime,
                CreatorUserId = cate.CreatorUserId


            }
        ).ToListAsync();
    }


    #region  GetPagedResult
    /// <summary>
    /// Get Paged Package
    /// </summary>
    public async Task<PagedResultDto<PackageDto>> GetPagedResultAsync(PagedPackageResultRequestDto input)
    {
        try
        {
            //   var query = _repository.GetAll();

            var data = await GetPackageDtoList();
            var query = data.AsQueryable();
            query = ApplyFilters(input, query);
            var totalCount = query.Count();
            var result = query.OrderByDescending(q => q.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return new PagedResultDto<PackageDto>(totalCount, result);
        }
        catch (Exception)
        {

            throw;
        }
    }


    #endregion


    #region Private Methods



    private async Task<List<PackageDto>> GetPackageDtoList()
    {
        UserDto sentUserInfo = new UserDto();
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;

        // Fetch all necessary data
        var packages = await _repository.GetAll()
                                         .Where(p => p.TenantId == tenantId)
                                         .ToListAsync();

        var users = await _userAppService.GetAllUsers();
        var packageTypes = await _packageRepository.GetAllListAsync();
        var subPackages = await _subPackageRepository.GetAllListAsync();

        // Construct a dictionary for faster lookups
        var userDictionary = users.Items.ToDictionary(u => u.Id);
        var packageTypeDictionary = packageTypes.ToDictionary(pt => pt.Id);

        var subPackageDictionary = subPackages.GroupBy(sp => sp.PackageId)
                                              .ToDictionary(g => g.Key, g => g.ToList());

        // Map packages to DTOs
        var packagesDtos = packages.Select(package =>
        {
            if (package.SenderId != null && package.SenderId > 0)
            {
                sentUserInfo = userDictionary.GetValueOrDefault((long)package.SenderId);
            }

            var packageType = packageTypeDictionary.GetValueOrDefault(package.PackageTypeId);

            // Convert subPackages to SubPackageDto
            var subPackagesForPackage = subPackageDictionary.GetValueOrDefault(package.Id)?
                .Select(subPackage => new SubPackageDto
                {
                    Id = subPackage.Id,
                    PackageId = subPackage.PackageId,
                    Contains = subPackage.Contains,
                    CreationTime = subPackage.CreationTime,
                    CreatorUserId = subPackage.CreatorUserId,
                    ImageUrl = subPackage.ImageUrl

                }).ToList() ?? new List<SubPackageDto>();


            // Map to PackageDto
            return new PackageDto
            {
                Id = package.Id,
                CreationTime = package.CreationTime,
                CreatorUserId = package.CreatorUserId,
                EmployeeId = package.EmployeeId,
                ImageUrl = package.ImageUrl,
                PackageTypeId = package.PackageTypeId,
                SenderId = package.SenderId,
                SenderFullName = package.SenderId != null ? $"{sentUserInfo.FullName}" : string.Empty,
                SenderEmail = sentUserInfo?.EmailAddress,
                SenderUserTypeId = (int)sentUserInfo?.UserTypeId,
                PackageTypeName = packageType?.Name,
                SubPackageDtos = subPackagesForPackage,
                ReceiveDate = package.Date,
                Description = package.Description,
                OuterSenderEmail = package.OuterSenderEmail,
                OuterSenderFirstName = package.OuterSenderFirstName,
                OuterSenderLastName = package.OuterSenderLastName,
                OuterSenderPhoneNumber = package.OuterSenderPhoneNumber,
                


            };
        }).ToList();

        return packagesDtos;
    }




    private static IQueryable<PackageDto> ApplyFilters(PagedPackageResultRequestDto input, IQueryable<PackageDto> query)
    {
        if (string.IsNullOrWhiteSpace(input.Keyword) == false)
        {
            query = query.Where(g => g.PackageTypeName.Contains(input.Keyword)
            || g.SenderEmail.Contains(input.Keyword)
            || g.SenderFullName.Contains(input.Keyword)
            );
        }

        if (input.PackageTypeId > 0)
        {
            query = query.Where(g => g.PackageTypeId == input.PackageTypeId);
        }

        if (input.UserTypeId > 0)
        {
            query = query.Where(g => g.SenderUserTypeId == input.UserTypeId);
        }

        if (!string.IsNullOrWhiteSpace(input.ReceiveDate))
        {
            string format = "ddd MMM dd yyyy";
            var datePart = input.ReceiveDate.Substring(0, 15);

            if (DateTime.TryParseExact(datePart, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                query = query.Where(g => g.ReceiveDate.Date == parsedDate.Date);
            }
        }

        if (!string.IsNullOrWhiteSpace(input.ToDate) && !string.IsNullOrWhiteSpace(input.FromDate))
        {
            string format = "ddd MMM dd yyyy";

            var toDatePart = input.ToDate.Substring(0, 15);   // Extract first part "Mon Sep 09 2024"
            var fromDatePart = input.FromDate.Substring(0, 15);

            // Try to parse both the ToDate and FromDate
            if (DateTime.TryParseExact(toDatePart, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedToDate) &&
                DateTime.TryParseExact(fromDatePart, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedFromDate))
            {
                // Ensure that parsedFromDate is earlier than parsedToDate
                if (parsedFromDate <= parsedToDate)
                {
                    // Apply range filter between FromDate and ToDate
                    query = query.Where(g => g.ReceiveDate.Date >= parsedFromDate.Date && g.ReceiveDate.Date <= parsedToDate.Date);
                }
            }
        }
        else if (!string.IsNullOrWhiteSpace(input.ToDate)) // Only filter by ToDate if FromDate is empty
        {
            string format = "ddd MMM dd yyyy";
            var toDatePart = input.ToDate.Substring(0, 15);

            if (DateTime.TryParseExact(toDatePart, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedToDate))
            {
                query = query.Where(g => g.ReceiveDate.Date <= parsedToDate.Date);
            }
        }
        else if (!string.IsNullOrWhiteSpace(input.FromDate)) // Only filter by FromDate if ToDate is empty
        {
            string format = "ddd MMM dd yyyy";
            var fromDatePart = input.FromDate.Substring(0, 15);

            if (DateTime.TryParseExact(fromDatePart, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedFromDate))
            {
                query = query.Where(g => g.ReceiveDate.Date >= parsedFromDate.Date);
            }
        }








        return query;
    }
    private static IQueryable<PackageDto> GetSelectQuery(IQueryable<PackageDto> query)
    {


        return query.Select(package => new PackageDto
        {
            Id = package.Id,
            CreationTime = package.CreationTime,
            CreatorUserId = package.CreatorUserId,
            EmployeeId = package.EmployeeId,
            ImageUrl = package.ImageUrl,
            PackageTypeId = package.PackageTypeId,
            SenderId = package.SenderId,
            SenderFullName = package.SenderFullName,
            SenderEmail = package.SenderEmail,
            SenderUserTypeId = package.SenderUserTypeId,
            PackageTypeName = package.PackageTypeName,
            SubPackageDtos = package.SubPackageDtos,
        });
    }

    private IQueryable<PackageDto> GetPackageSelectQuery(IQueryable<Webminux.Optician.Package.Pacakge> query)
    {
        return query.Select(b => new PackageDto
        {
            Id = b.Id,
            CreatorUserId = b.CreatorUserId,
            CreationTime = b.CreationTime,

        });
    }



    #endregion
}