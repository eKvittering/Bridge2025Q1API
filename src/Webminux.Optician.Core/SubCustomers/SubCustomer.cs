using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.Notes;

namespace Webminux.Optician.Core.SubCustomers
{
    public class SubCustomer : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Phone { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Email { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Address { get; set; }

        [ForeignKey(nameof(Customer))]
        public virtual int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        protected SubCustomer() { }

        public static SubCustomer Create(int tenantId, string name, string email, string phone, string address, int customerId)
        {
            return new SubCustomer
            {
                TenantId = tenantId,
                Name = name,
                Email = email,
                Phone = phone,
                Address = address,
                CustomerId = customerId
            };
        }
    }
}