using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Webminux.Optician.Users.Dto;

namespace Webminux.Optician.Application.Customers.Dtos
{
    /// <summary>
    /// This is a DTO class for Customer entity.
    /// </summary>
    public class CreateCustomerDto : CreateUserDto
    {
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
        /// Employee who is responsible to handle this customer.
        /// </summary>
        public virtual long ResponsibleEmployeeId { get; set; }

        /// <summary>
        /// CustomerTypeId
        /// </summary>
        public virtual int? CustomeTypeId { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public virtual int? ParentId { get; set; }

        /// <summary>
        /// Is Sub cusotmer
        /// </summary>
        public virtual bool IsSubCustomer { get; set; }

        /// <summary>
        /// Site. 
        /// </summary>
        public virtual string Site { get; set; } = string.Empty;
        /// <summary>
        /// Custom Fields for customer entity
        /// </summary>
        public virtual ICollection<EntityFieldMappingDto> CustomFields { get; set; }

        /// <summary>
        /// Tenant id is optional
        /// </summary>
        public int TenantId { get; set; } = 0;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CreateCustomerDto()
        {
            CustomFields = new HashSet<EntityFieldMappingDto>();
        }

    }
}