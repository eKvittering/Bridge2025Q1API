namespace Webminux.Optician.Application
{
    /// <summary>
    /// Dto to get Activities list with data filtration.
    /// </summary>
    public class GetActivitiesInputDto
    {
        /// <summary>
        /// From Date to filter activities.
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// To Date to filter activities.
        /// </summary>
        public string ToDate { get; set; }

        /// <summary>
        /// Activity Room Id.
        /// </summary>
        public int? RoomId { get; set; }
    }
}