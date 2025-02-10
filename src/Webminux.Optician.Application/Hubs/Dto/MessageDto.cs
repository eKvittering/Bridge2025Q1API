using System;

namespace Webminux.Optician.Hubs.Dto
{
    public class MessageDto
    {
        public long SenderId { get; set; }
        public int ReceiverTenantId { get; set; }
        public long ReceiverId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsAdmin { get; set; }
        public int SenderTenantId { get; set; }
    }
}
