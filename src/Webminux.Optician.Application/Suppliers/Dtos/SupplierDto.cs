using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Users.Dto;

namespace Webminux.Optician.Application.Suppliers.Dtos
{
    /// <summary>
    /// This is a DTO class for <see cref="Customer"/> entity.
    /// </summary>
    public class SupplierDto:EntityDto,ICreationAudited
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
        /// Gets or sets the phone number.
        /// </summary>
        public string[] RoleNames { get; set; }

        /// <summary>
        /// Number to Indentify Supplier
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string SupplierNo { get; set; }

        /// <summary>
        /// Streat Address of Supplier
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Address { get; set; }

        /// <summary>
        /// Postal Code 
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Postcode { get; set; }

        /// <summary>
        /// Town or City Name
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string TownCity { get; set; }

        /// <summary>
        /// Telephone Number
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Telephone { get; set; }

        /// <summary>
        /// Fax 
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Fax { get; set; }

        /// <summary>
        /// Website that represent supplier business
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Website { get; set; }

        /// <summary>
        /// Gets or sets the User Id.
        /// </summary>
        public virtual long UserId { get; set; }
    }
}