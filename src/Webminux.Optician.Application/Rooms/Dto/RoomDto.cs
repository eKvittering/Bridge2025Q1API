using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Webminux.Optician;

namespace Webminux.Optician
{
    /// <summary>
    /// Contains Room Data
    /// </summary>
    public class RoomDto : CreationAuditedEntityDto
    {
        ///<summery>
        /// Name of Room
        ///</summery>
        public virtual string Name { get; set; }

        ///<summery>
        /// Additional Information About Room
        ///</summery>
        public virtual string Descriptions { get; set; }
    }
}