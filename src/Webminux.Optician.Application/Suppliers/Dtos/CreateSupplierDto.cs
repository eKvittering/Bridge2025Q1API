using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Webminux.Optician.CustomFields;
using Webminux.Optician.Users.Dto;

namespace Webminux.Optician.Application.Suppliers.Dtos
{
    /// <summary>
    /// Data Trasfer Object To create supplier
    /// </summary>
    public class CreateSupplierDto : CreateUserDto
    {
        /// <summary>
        /// Number to Indentify Supplier
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string SuplierNo { get; set; }
        
        /// <summary>
        /// Streat Address of Supplier
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Address { get; set; }

        /// <summary>
        /// Postal Code 
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Postcode { get; set; }

        /// <summary>
        /// Town or City Name
        /// </summary>
        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string TownCity { get; set; }

        /// <summary>
        /// Telephone Number
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Telephone { get; set; }

        /// <summary>
        /// Fax 
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Fax { get; set; }

        /// <summary>
        /// Website that represent supplier business
        /// </summary>
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Website { get; set; }
        public int TenantId { get;  set; }
    }
}