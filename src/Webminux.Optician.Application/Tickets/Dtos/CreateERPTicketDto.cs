namespace Webminux.Optician.Tickets.Dtos
{
    public class CreateERPTicketDto : CreateTicketDto
    {
        public int? CustomerUserId { get; set; }
        public string CustomerNo { get; set; }
        public int? TenantId { get; set; }
        public long EmployeeId { get; set; }
    }
}
