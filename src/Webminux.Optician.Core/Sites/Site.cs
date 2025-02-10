using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace Webminux.Optician.Sites
{
    public class Site : CreationAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public bool IsMedicalType { get; set; }
        public string InvoiceCurrency { get; set; }
        public string Notes { get; set; }



        public Site()
        {
             
        }

        public Site Create(int tenantId, string name, string address , string postalCode ,string country ,string phone,bool isMedicalType , string notes,string invoiceCurrency,long? createdUserId)
        {
            return new Site()
            {
                TenantId = tenantId,
                Name = name,
                CreationTime = DateTime.Now,
                Address = address ,
                PostalCode = postalCode ,
                Country = country,
                Phone = phone ,
                IsMedicalType = isMedicalType,
                Notes = notes ,
                InvoiceCurrency = invoiceCurrency ,
                CreatorUserId = createdUserId,
            };
        }
    }
}

