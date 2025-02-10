using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;

namespace Webminux.Optician.Suppliers
{
    public class Supplier:FullAuditedEntity,IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string SupplierNo { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Address { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Postcode { get; set; }

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string TownCity { get; set; }


        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Telephone { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Fax { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Website { get; set; }

        [ForeignKey(nameof(User))]
        public virtual long UserId { get; set; }
        public virtual User User { get; set; }

        protected Supplier() { }

        public static Supplier Create(int tenantId,string supplierNo,string address,string postcode,string townCity,string telephone,string fax,string website,long userId)
        {
            var supplier = new Supplier
            {
                TenantId = tenantId,
                SupplierNo = supplierNo,
                Address = address,
                Postcode = postcode,
                TownCity = townCity,
                Telephone = telephone,
                Fax = fax,
                Website = website,
                UserId=userId
            };
            return supplier;
        }
    }
}
