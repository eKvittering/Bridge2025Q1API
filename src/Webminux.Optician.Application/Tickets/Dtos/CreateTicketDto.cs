namespace Webminux.Optician.Tickets.Dtos
{
    /// <summary>
    /// Data transfer object to create fault
    /// </summary>
    public class CreateTicketDto
    {
        /// <summary>
        /// Information provided by client about fault
        /// </summary>

        public virtual int Status { get; set; }

        /// <summary>
        /// Information provided by client about fault
        /// </summary>
        public virtual string Comment { get; set; }


        /// <summary>
        /// Information provided by client about fault
        /// </summary>
        public virtual string Description{ get; set; }

        /// <summary>
        /// Email Provided by client 
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Date of fault.
        /// </summary>
        public virtual string Date { get; set; }

        /// <summary>
        /// Base 64 string of image.
        /// </summary>
        public virtual string Base64ImageString { get; set; }

        public virtual int? GroupId { get; set; }

        /// <summary>
        /// Id of Product Item.
        /// </summary>
        public virtual int? CustomerId { get; set; }
        public virtual long? EmployeeId { get; set; } = 0;

    }
}
