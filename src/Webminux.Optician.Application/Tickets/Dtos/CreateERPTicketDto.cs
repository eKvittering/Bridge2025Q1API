namespace Webminux.Optician.Tickets.Dtos
{
    public class CreateERPTicketDto : CreateTicketDto
    {
        public int? CustomerUserId { get; set; }
        public long EmployeeId { get; set; }
    }
}
