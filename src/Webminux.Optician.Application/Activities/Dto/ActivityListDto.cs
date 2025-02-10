namespace Webminux.Optician
{
    /// <summary>
    /// Data transfer object for Activities listing.
    /// </summary>
    public class ActivityListDto : ActivityDto
    {
        /// <summary>
        /// Gets or sets the Customer Name.
        /// </summary>
        public virtual string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the Cutomer Email.
        /// </summary>
        public virtual string CustomerEmail { get; set; }

        /// <summary>
        /// Gets or sets the Customer User Id.
        /// </summary>
        public virtual long? CustomerUserId { get; set; }

        /// <summary>
        /// Gets or sets the Follow Up Type Name.
        /// </summary>
        public virtual string FollowUpTypeName { get; set; }

        /// <summary>
        /// Gets or sets the Activity Type Name.
        /// </summary>
        public virtual string ActivityTypeName { get; set; }

        /// <summary>
        /// Gets or sets the Activity Art Name.
        /// </summary>
        public virtual string ActivityArtName { get; set; }

        /// <summary>
        /// Gets or sets the Employee Name.
        /// </summary>
        public virtual string EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the Room Name.
        /// </summary>
        public virtual string RoomName { get; set; }

        /// <summary>
        /// Gets or sets the Product Item Id.
        /// </summary>
        public virtual int? ProductItemId { get; set; }

        /// <summary>
        /// Gets or sets the Product SerialNumber.
        /// </summary>
        public virtual string ProductSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the Product Name.
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the Product Number.
        /// </summary>
        public virtual string ProductNumber { get; set; }

        /// <summary>
        /// Get or sets the supplier Id
        /// </summary>
        public virtual int? SupplierId { get; set; }

        /// <summary>
        /// Name of supplier
        /// </summary>
        public virtual string SupplierName { get; set; }
        public virtual string? GroupName { get; set; }

        /// <summary>
        /// Get or sets the ticket Id
        /// </summary>
        public virtual int? TicketId { get; set; }

        public virtual string CustomerType { get; set; }
    }
}
