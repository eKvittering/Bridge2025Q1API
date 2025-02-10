using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Brands;
using Webminux.Optician.Categories;
using Webminux.Optician.Products;
using Webminux.Optician.Suppliers;

namespace Webminux.Optician
{
    /// <summary>
    /// ProductDto
    /// </summary>
    public class Product : FullAuditedEntity, IMustHaveTenant, ILookupDto<int>
    {
        public virtual int TenantId { get; set; }

        /// <summary>
        /// Product No
        /// </summary>

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string ProductNumber { get; set; }

        /// <summary>
        /// Name
        /// </summary>

        [Required]
        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product Group Number
        /// </summary>
        public virtual int? ProductGroupNumber { get; set; }



        /// <summary>
        /// SalesPrice
        /// </summary>
        public virtual double SalesPrice { get; set; }

        /// <summary>
        /// CostPrice
        /// </summary>
        public virtual double CostPrice { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        public virtual double Unit { get; set; }

        /// <summary>
        /// <see cref="Barcode"/>
        /// </summary>
        public virtual string Barcode { get; set; }

        /// <summary>
        /// Access
        /// </summary>
        public virtual string Access { get; set; }

        /// <summary>
        /// Rec price
        /// </summary>
        public virtual string Recprice { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public virtual string Category { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public virtual string Location { get; set; }

        /// <summary>
        /// GrossWeight
        /// </summary>
        public virtual string GrossWeight { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public virtual string Volume { get; set; }

        /// <summary>
        /// ProductDiscountGroup
        /// </summary>

        public virtual string ProductDiscountGroup { get; set; }

        /// <summary>
        ///  minimum stock 
        /// </summary>
        public virtual int MinStock { get; set; }

        /// <summary>
        ///  minimum order 
        /// </summary>
        public virtual int MinOrder { get; set; }

        /// <summary>
        /// RecCostPrice
        /// </summary>
        public virtual double RecCostPrice { get; set; }

        /// <summary>
        /// InStock
        /// </summary>
        public virtual int InStock { get; set; }

        /// <summary>
        /// Picture URL Provided by cloudinary
        /// </summary>
        public virtual string PictureUrl { get; set; }

        public virtual bool IsMedicalDevice { get; set; }
        /// <summary>
        /// Picture Public Id Provided by cloudinary
        /// </summary>
        public virtual string PicturePublicId { get; set; }
        public virtual int? CategoryId { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual int? BrandId { get; set; }
        public virtual Brand Brand { get; set; }

        [ForeignKey(nameof(Employee))]
        public virtual long? EmployeeId { get; set; }
        public virtual User Employee { get; set; }

        [ForeignKey(nameof(Supplier))]
        public virtual int? SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<Webminux.Optician.ProductItem.ProductItem> ProductSerials { get; set; }

        public virtual ICollection<ProductResponsibleGroup> ResponsibleGroups { get; set; }

        public Product()
        {

        }


        public static Product Create(bool ismedicalDevice, int tenantId, string productNumber, string name, string description,
            int? productGroupNumber, double salesPrice, double costPrice, double unit, string barcode,
            string access, string recprice, string category, string location, string grossWeight,
            string volume, string productDiscountGroup, int minStock, int minOrder, double recCostPrice,
            int inStock, string pictureUrl, string picturePublicId, int? categoryId, int? brandId, int? supplierId, long? employeeId)
        {
            var product = new Product
            {
                ProductNumber = productNumber,
                Name = name,
                Description = description,
                ProductGroupNumber = productGroupNumber,
                SalesPrice = salesPrice,
                CostPrice = costPrice,
                Unit = unit,
                Barcode = barcode,
                Access = access,
                Recprice = recprice,
                Category = category,
                Location = location,
                GrossWeight = grossWeight,
                Volume = volume,
                ProductDiscountGroup = productDiscountGroup,
                MinStock = minStock,
                MinOrder = minOrder,
                RecCostPrice = recCostPrice,
                TenantId = tenantId,
                InStock = inStock,
                PictureUrl = pictureUrl,
                PicturePublicId = picturePublicId,
                CategoryId = categoryId,
                BrandId = brandId,
                SupplierId = supplierId,
                EmployeeId = employeeId,
                IsMedicalDevice = ismedicalDevice
            };

            return product;
        }

    }
}
