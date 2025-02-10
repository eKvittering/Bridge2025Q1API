using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;
using Webminux.Optician.Authorization.Users;

namespace Webminux.Optician.Users.Dto
{
    /// <summary>
    /// A DTO class that can be used to transfer user data.
    /// </summary>
    [AutoMapTo(typeof(User))]
    public class
    CreateUserDto : IShouldNormalize
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
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; } = string.Empty;

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
        public string[] RoleNames { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public int UserTypeId { get; set; }

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

        public string Base64Picture { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        /// <summary>
        /// Normalize role names.
        /// </summary>
        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }
}
