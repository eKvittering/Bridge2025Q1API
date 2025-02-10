using System.Collections.Generic;

namespace Webminux.Optician.BackgroundJobs.Dto
{
    public class BillyProductApiResponseDto
    {
        public BillyMetaDataDto meta { get; set; }
        public List<BillyProductDto> products { get; set; }
    }


}
