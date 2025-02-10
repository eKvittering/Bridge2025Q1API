using System.Collections.Generic;

namespace Webminux.Optician.BackgroundJobs.Dto
{
    public class BillyContactApiResponseDto
    {
        public BillyMetaDataDto meta { get; set; }
        public List<BillyContactDto> contacts { get; set; }
    }
}
