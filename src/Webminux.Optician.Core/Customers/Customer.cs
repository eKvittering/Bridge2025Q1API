using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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


       // =============================== Client Properties==================

        public int? NUMMER { get; set; }

        /// <summary>
        /// Date the member was created.
        /// </summary>
        public DateTime? OPRETTET { get; set; }

        /// <summary>
        /// Date the member data was last changed.
        /// </summary>
        public DateTime? AENDRET { get; set; }

        /// <summary>
        /// First name of the member.
        /// </summary>
        public string FORNAVN { get; set; }

        /// <summary>
        /// Middle name of the member.
        /// </summary>
        public string MELLEMNAVN { get; set; }

        /// <summary>
        /// Last name (surname) of the member.
        /// </summary>
        public string EFTERNAVN { get; set; }

        /// <summary>
        /// Primary address line (Address).
        /// </summary>
        public string ADRESSE1 { get; set; }

        /// <summary>
        /// Secondary address line (optional).
        /// </summary>
        public string ADRESSE2 { get; set; }

        /// <summary>
        /// Private phone number.
        /// </summary>
        public string PRIVATTELEFON { get; set; }

        /// <summary>
        /// Work phone number.
        /// </summary>
        public string ARBEJDSTELEFON { get; set; }

        /// <summary>
        /// Mobile phone number (TelephoneFax).
        /// </summary>
        public string MOBILTELEFON { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        public DateTime? FOEDSELSDAG { get; set; }

        /// <summary>
        /// Notes about the member (stored as image/blob in database).
        /// </summary>
        public byte[] NOTER { get; set; }

        /// <summary>
        /// Possibly year of membership or joining.
        /// </summary>
        public short? K1AAR { get; set; }

        /// <summary>
        /// Does not wish to appear in the address book (flag).
        /// </summary>
        public short? DB_OENSKERIKKE { get; set; }

        /// <summary>
        /// Always send printed material (flag).
        /// </summary>
        public short? DB_SENDALTID { get; set; }

        /// <summary>
        /// Invalid address flag.
        /// </summary>
        public short? DB_UGYLDIG_ADRESSE { get; set; }

        /// <summary>
        /// Indicates if the family receives the magazine.
        /// </summary>
        public short? DB_FAMILIEN_FAAR_BLADET { get; set; }

        /// <summary>
        /// Indicates if the member is deceased.
        /// </summary>
        public short? DOED { get; set; }

        /// <summary>
        /// Wishes to receive a badge/pin (flag).
        /// </summary>
        public short? NAAL_OENSKES { get; set; }

        /// <summary>
        /// Should be updated in the WinFinans system (flag).
        /// </summary>
        public short? OPDATERWINFINANS { get; set; }

        /// <summary>
        /// Foreign key to country code (Country).
        /// </summary>
        public string FKLANDEKODE { get; set; }

        /// <summary>
        /// Foreign key to postal code (Postcode).
        /// </summary>
        public string FKPOSTNR { get; set; }

        /// <summary>
        /// Indicates if the member maintains their own profile data.
        /// </summary>
        public short? VEDLIGEHOLDER_EGNE_STAMDATA { get; set; }

        /// <summary>
        /// Web access code or password (Website).
        /// </summary>
        public string WEB_ADGANGSKODE { get; set; }

        /// <summary>
        /// Member title (foreign key to title table).
        /// </summary>
        public int? MPTITEL { get; set; }

        /// <summary>
        /// Last time the title was changed.
        /// </summary>
        public DateTime? MPTITEL_AENDRET { get; set; }

        /// <summary>
        /// Possibly handicap index or similar rating.
        /// </summary>
        public double? NUVHAC { get; set; }

        /// <summary>
        /// Has an alternative address (flag).
        /// </summary>
        public short? ALTERNATIV_ADRESSE { get; set; }

        /// <summary>
        /// Trial flag or test field (likely temporary/test purpose).
        /// </summary>
        public char? TRIAL675 { get; set; }



        // =============================== Client Properties End==================




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