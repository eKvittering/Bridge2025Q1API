namespace Webminux.Optician.BackgroundJobs.Dto
{
    public class BillyMetaDataDto
    {
        public int statusCode { get; set; }
        public bool success { get; set; }
        public BillyPagingDto paging { get; set; }
        public double time { get; set; }
    }

}
