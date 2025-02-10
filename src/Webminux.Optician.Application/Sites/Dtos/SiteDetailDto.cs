using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Sites.Dtos
{
    /// <summary>
    /// This is a DTO class for Site Detail.
    /// </summary>
    public class SiteDetailDto : EntityDto
    {
       
        public virtual string Name { get; set; }
        public virtual int TenantId { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public bool IsMedicalType { get; set; }
        public string InvoiceCurrency { get; set; }
        public string Notes { get; set; }
        public DateTime CreationTime { get;  set; }
        public long? CreatorUserId { get;  set; }
    }
}