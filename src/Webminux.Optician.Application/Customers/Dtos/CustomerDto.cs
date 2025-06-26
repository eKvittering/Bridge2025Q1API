using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webminux.Optician.Customers.Dtos
{
    /// <summary>
    /// This is a DTO class for <see cref="Customer"/> entity.
    /// </summary>
    public class CustomerDto : EntityDto, ICreationAudited
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
        /// Gets or sets the customer no.
        /// </summary>
        [Required]
        public virtual string CustomerNo { get; set; }

        /// <summary>
        /// Gets or sets the customer Address.
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Address { get; set; }

        /// <summary>
        /// Gets or sets the customer Postcode.
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the customer TownCity.
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string TownCity { get; set; }

        /// <summary>
        /// Gets or sets the customer Country.
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Country { get; set; }

        /// <summary>
        /// Gets or sets the customer TelephoneFax.
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string TelephoneFax { get; set; }

        /// <summary>
        /// Gets or sets the customer Website.
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Website { get; set; }

        /// <summary>
        /// Gets or sets the customer Currency.
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Currency { get; set; }

        /// <summary>
        /// Gets or sets the User Id.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Employee who is responsible to handle this customer.
        /// </summary>
        public virtual long? ResponsibleEmployeeId { get; set; }

        /// <summary>
        /// Parent id.
        /// </summary>
        public virtual int? ParentId { get; set; }

        /// <summary>
        /// Parent id.
        /// </summary>
        public virtual int? CustomerTypeId { get; set; }

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

        public virtual bool IsSubCustomer { get; set; }

        public string Base64Picture { get; set; }

        public bool IsSubCustomerOpen { get; set; } = true;
        public string CustomerType { get; set; }
        public string Site { get; set; }
        public int TenantId { get; set; }

        public string ResponsibleEmployee { get; set; }

        public ICollection<CustomerDto> SubCustomers { get; set; } = new List<CustomerDto>();

        /// <summary>
        /// Custom Fields for customer entity
        /// </summary>
        public virtual ICollection<EntityFieldMappingDto> CustomFields { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomerDto()
        {
            CustomFields = new HashSet<EntityFieldMappingDto>();
        }
    }
}