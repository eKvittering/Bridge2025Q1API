using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Suppliers.Dtos
{
    /// <summary>
    /// This is a DTO class for Customer Listing.
    /// </summary>
    public class SupplierListDto:EntityDto
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
        ///  supplier user id
        /// </summary>
        public long SupplierUserId { get; set; } = 0;
    }
}