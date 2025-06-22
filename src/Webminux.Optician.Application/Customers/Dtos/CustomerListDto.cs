using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Customers.Dtos
{
    /// <summary>
    /// This is a DTO class for Customer Listing.
    /// </summary>
    public class CustomerListDto:EntityDto
    {
        /// <summary>
        /// Gets or sets the customer no.
        /// </summary>
        public virtual string CustomerNo { get; set; }
       
        /// <summary>
        /// Gets or sets the customer Name.
        /// </summary>
        public virtual string Name { get; set; }
       
        /// <summary>
        /// Gets or sets the customer EmailAddress.
        /// </summary>
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// UserId Against customer
        /// </summary>
        public virtual long CustomerUserId { get; set; }
        public string? Address { get;  set; }
        public string? Postcode { get;  set; }
        public string? TownCity { get;  set; }
        public string? Country { get;  set; }
        public string? TelephoneFax { get;  set; }
        public string? Website { get;  set; }
        public string? Currency { get;  set; }
        public long? UserId { get;  set; }
        public string? UserName { get;  set; }
        public string? Surname { get;  set; }
        public int? UserTypeId { get;  set; }
        public string TenantName { get; set; }
        public int? TenantId { get; set; }

    }
}