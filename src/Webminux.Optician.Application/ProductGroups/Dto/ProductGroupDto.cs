using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;

namespace Webminux.Optician.ProductGroups.Dto
{
    /// <summary>
    ///  product group dto
    /// </summary>
    public class ProductGroupDto : EntityDto, ICreationAudited
    {
        public int ProductGroupNumber { get; set; }
        public string Name { get; set; }
        public string Domestic { get; set; }
        public string EU { get; set; }
        public string Abroad { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }

    }
}
