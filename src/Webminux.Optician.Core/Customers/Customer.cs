using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Notes;
using Webminux.Optician.Customers;

namespace Webminux.Optician.Core.Customers
{
    [Index(nameof(CustomerNo))] 
    [Index(nameof(Address))] 
    [Index(nameof(Postcode))] 
    [Index(nameof(TelephoneFax))] 
    [Index(nameof(TenantId))] 
    [Index(nameof(UserId))] 
    [Index(nameof(ResponsibleEmployeeId))] 
    [Index(nameof(CustomeTypeId))]
    [Index(nameof(ParentId))] 
    [Index(nameof(IsSubCustomer))] 
    
    
    public class Customer : Entity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string CustomerNo { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Address { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Postcode { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string TownCity { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Country { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string TelephoneFax { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Website { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Currency { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Site { get; set; }
        public virtual bool IsSync { get; set; }

        public virtual bool IsSubCustomer { get; set; }

        [ForeignKey(nameof(User))]
        public virtual long UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(ResponsibleEmployee))]
        public virtual long? ResponsibleEmployeeId { get; set; }
        public virtual User ResponsibleEmployee { get; set; }

        [ForeignKey(nameof(CustomerType))]
        public virtual int? CustomeTypeId { get; set; }
        public virtual CustomerType CustomerType { get; set; }

        [ForeignKey(nameof(ParentCustomer))]
        public virtual int? ParentId { get; set; }
        [JsonIgnore]
        public virtual Customer ParentCustomer { get; set; }
        [JsonIgnore]
        public virtual ICollection<Customer> SubCustomers { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Note> Notes { get; set; }

        [JsonConstructor]
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
            Notes = new HashSet<Note>();
        }

        public static Customer Create(int tenantId, string customerNo, string address, string postCode,
            string townCity, string country, string telephoneFax, string website,
            string currency, long userId, long? responsibleEmployeeId,
            int? customerType, int? parentId, bool isSubcustomer, string site, bool isSync = false)
        {
            return new Customer
            {
                TenantId = tenantId,
                CustomerNo = customerNo,
                Address = address,
                Postcode = postCode,
                TownCity = townCity,
                Country = country,
                TelephoneFax = telephoneFax,
                Website = website,
                Currency = currency,
                UserId = userId,
                ResponsibleEmployeeId = responsibleEmployeeId,
                IsSync = isSync,
                CustomeTypeId = customerType,
                ParentId = parentId,
                Site = site,
                IsSubCustomer = isSubcustomer,
            };
        }
    }
}