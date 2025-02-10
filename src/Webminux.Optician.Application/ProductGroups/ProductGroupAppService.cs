using Abp.Application.Services.Dto;
using Abp.UI;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.Helpers;
using Webminux.Optician.ProductGroups;
using Webminux.Optician.ProductGroups.Dto;
using Webminux.Optician.Products.Dto;


namespace Webminux.Optician.Products
{
    /// <summary>
    /// The class that represents a product.
    /// </summary>
    public class ProductGroupAppService : OpticianAppServiceBase, IProductItemAppService
    {

        private readonly IProductGroupManager _productGroupManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAppService"/> class.
        /// </summary>
        /// <param name="productGroupManager"></param>
        public ProductGroupAppService(IProductGroupManager productGroupManager)
        {
            this._productGroupManager = productGroupManager;
        }

        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="input"></param>
      
        public async Task Create(ProductGroupDto input)
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
              await _productGroupManager.CreateAsync(GetProductGroupModel(input, tenantId));

        }

        /// <summary>
        ///  Gets the product by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductGroupDto> GetById(int id)
        {
            var data = await _productGroupManager.GetAsync(id);
            return ObjectMapper.Map<ProductGroupDto>(data);
        }

        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task Update(ProductGroupDto input)
        {
            var productGroupDb = await _productGroupManager.GetAsync(input.Id);
            if (productGroupDb == null)
            {
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.ActivityNotFound);
            }
            ObjectMapper.Map(input, productGroupDb);
        }

        

        /// <summary>
        ///  get all product 
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<LookUpDto<int>>> GetAll()
        {
          return await  _productGroupManager.GetAll().GetLookupResultAsync<ProductGroup, int>();
        }


        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="input"></param>
      
        public async Task Delete(EntityDto input)
        {
            var data = await _productGroupManager.GetAsync(input.Id);
            if (data == null)
            {
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.ActivityNotFound);
            }

            await _productGroupManager.DeleteAsync(data);
        }


        private static ProductGroup GetProductGroupModel(ProductGroupDto input, int tenantid)
        {
            return ProductGroup.Create(input.ProductGroupNumber, input.Name, input.Domestic, input.EU, input.Abroad, tenantid);
        }


        /// <summary>
        ///  Gets the products with pagination.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductGroupDto>> GetPagedResultAsync(PagedProductGroupResultRequestDto input)
        {

            var query = _productGroupManager.GetAll();
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(x => x.Name.Contains(input.Keyword)
                || x.Abroad.Contains(input.Keyword) 
                || x.EU.Contains(input.Keyword) 
                || x.Domestic.Contains(input.Keyword));
            }

            IQueryable<ProductGroupDto> selectQuery = GetSelectQueryForProducGrouptList(query);
            var result = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            return result;
        }

        private static IQueryable<ProductGroupDto> GetSelectQueryForProducGrouptList(IQueryable<ProductGroup> query)
        {
            return query.Select(x =>  new ProductGroupDto
            {
               
                Id = x.Id,
                Name = x.Name,
                CreatorUserId = x.CreatorUserId,
                CreationTime = x.CreationTime,
                Abroad = x.Abroad,
                ProductGroupNumber = x.ProductGroupNumber,
                Domestic = x.Domestic,
                EU = x.EU,
                
               


            }); 
        }
    }
}
