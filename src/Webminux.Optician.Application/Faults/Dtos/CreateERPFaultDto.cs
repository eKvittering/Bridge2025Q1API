namespace Webminux.Optician.Faults.Dtos
{
    public class CreateERPFaultDto : CreateFaultDto
    {
        public long CustomerUserId { get; set; }
        public long? ResposibleEmployeeId { get; set; }
    }
}
