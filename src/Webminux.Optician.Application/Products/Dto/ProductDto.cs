using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Products.Dto;
using Webminux.Optician.Suppliers;

namespace Webminux.Optician.Products.Dtos
{
    /// <summary>
    /// ProductDto
    /// </summary>
    public class ProductDto : EntityDto, ICreationAudited
    {
        ///// <summary>
        ///// primay ky
        ///// </summary>
        //[Key]
        //public int Id { get; set; }
        /// <summary>
        /// Prodcut No
        /// </summary>
        public string ProductNumber { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product Group Number
        /// </summary>
        public int? ProductGroupNumber { get; set; }


        /// <summary>
        /// SalesPrice
        /// </summary>
        public double SalesPrice { get; set; }

        /// <summary>
        /// CostPrice
        /// </summary>
        public double CostPrice { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        public double Unit { get; set; }

        /// <summary>
        /// Barcode
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Access
        /// </summary>
        public string Access { get; set; }

        /// <summary>
        /// Rec price
        /// </summary>
        public string Recprice { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// GrossWeight
        /// </summary>
        public string GrossWeight { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// ProductDiscountGroup
        /// </summary>

        public string ProductDiscountGroup { get; set; }

        /// <summary>
        ///  minmum stock 
        /// </summary>
        public int MinStock { get; set; }

        /// <summary>
        ///  minmum order 
        /// </summary>
        public int MinOrder { get; set; }

        /// <summary>
        /// RecCostPrice
        /// </summary>
        public double RecCostPrice { get; set; }
        /// <summary>
        /// Gets or sets Creator User Id.
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// Gets or sets Creation Time.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        ///  in stock
        /// </summary>
        public int InStock { get; set; }

        /// <summary>
        /// Base64 String of uploaded picture
        /// </summary>
        public string Base64Picture { get; set; }

        /// <summary>
        /// Custom Fields for customer entity
        /// </summary>
        /// 
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }

        public List<int> ResponsibleGroupIds{ get; set; }

        public virtual List<ProductResponsibleGroup>? ResponsibleGroups { get; set; }

        public int SupplierId { get; set; }
        public virtual bool IsMedicalDevice { get; set; }

        public List<String> ProductSerials { get; set; }
        public string PictureUrl { get; set; }
        public List<ProductSerial> Serials { get; set; }
        public virtual ICollection<EntityFieldMappingDto> CustomFields { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductDto()
        {
            CustomFields = new HashSet<EntityFieldMappingDto>();
        }
    }
}
