using Abp.Application.Services.Dto;

namespace Webminux.Optician.Products.Dto
{
    /// <summary>
    ///  details product dto
    /// </summary>
    public class DetailsProductDto : EntityDto
    {
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
        public int ProductGroupNumber { get; set; }



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
     

    }
}
