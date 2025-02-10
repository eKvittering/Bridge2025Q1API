namespace Webminux.Optician.Application.Users.Dto
{
    /// <summary>
    /// Transfer data for User Listing
    /// </summary>
    public class GetAllInputDto
    {
        /// <summary>
        /// Type of User to filter
        /// </summary>
        public string UserType { get; set; }
    }
}