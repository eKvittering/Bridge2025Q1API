using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Authorization.Users;

namespace Webminux.Optician.Users.Dto
{
    /// <summary>
    /// A DTO class that can be used to transfer user data.
    /// </summary>
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>, ICreationAudited
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name of the full.
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets Creator UserId.
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// Can employee open or close shop.
        /// </summary>
        public virtual bool CanOpenCloseShop { get; set; }

        /// <summary>
        /// Is employee responsible for stocks.
        /// </summary>
        public virtual bool IsResponsibleForStocks { get; set; }

        /// <summary>
        /// Is reception allowed for employee.
        /// </summary>
        public virtual bool IsReceptionAllowed { get; set; }

        /// <summary>
        /// Can employee answer customer phone calls.
        /// </summary>
        public virtual bool CanAnswerPhoneCalls { get; set; }

        public virtual string PictureUrl { get; set; }
      
        /// <summary>
        /// TenantId of customer
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string[] RoleNames { get; set; }
    }
}
