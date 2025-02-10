using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ProductItem.Dtos
{
    public class SaleProductItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productItemId"></param>
        public SaleProductItem(int productId, int productItemId)
        {
            ProductId = productId;
            ProductItemId = productItemId;
        }

        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
    }
}
