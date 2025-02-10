using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.CustomFields;
using Webminux.Optician.Helpers;
using Webminux.Optician.Products.Dto;
using Webminux.Optician.Products.Dtos;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Products
{
    /// <summary>
    /// The class that represents a product.
    /// </summary>
    public class ProductAppService : OpticianAppServiceBase, IProductAppService
    {

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Webminux.Optician.ProductItem.ProductItem> _productSerialNoRepository;
        private readonly IMediaHelperService _imageHelperService;
        private readonly ICustomFieldManager _customFieldManager;
        private readonly IRepository<ProductResponsibleGroup> _responsibleGroupRepository;
        private readonly IRepository<EmployeeGroup> _employeeGroupRepository;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductAppService(
            IRepository<Product> productRepository,
            IRepository<Webminux.Optician.ProductItem.ProductItem> productSerialNoRepository,
            IMediaHelperService imageHelperService,
            ICustomFieldManager customFieldManager,
            IRepository<ProductResponsibleGroup> responsibleGroupRepository,
            IRepository<EmployeeGroup> employeeGroupRepository)
        {
            _productRepository = productRepository;
            _productSerialNoRepository = productSerialNoRepository;
            _imageHelperService = imageHelperService;
            _customFieldManager = customFieldManager;
            _responsibleGroupRepository = responsibleGroupRepository;
            _employeeGroupRepository = employeeGroupRepository;
        }

        #region Create
        /// <summary>
        /// Create New Product
        /// </summary>
        /// <param name="input"></param>
        public async Task CreateAsync(ProductDto input)
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;

            MediaUploadDto uploadResult = new MediaUploadDto();
            if (!string.IsNullOrWhiteSpace(input.Base64Picture))
                uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);

            var product = GetProductActivityModel(input, tenantId, uploadResult.Url, uploadResult.PublicId);
            product.ProductNumber = (_productRepository.Count() + 1).ToString();
            var productId = await _productRepository.InsertAndGetIdAsync(product);
            foreach (var groupId in input.ResponsibleGroupIds)
            {
                await _responsibleGroupRepository.InsertAsync(ProductResponsibleGroup.Create(productId, groupId));
            }
            await UnitOfWorkManager.Current.SaveChangesAsync();
            await _customFieldManager.CreateEntityFieldMappings(input.CustomFields, tenantId, product.Id);
        }
        #endregion

        #region GetById
        /// <summary>
        ///  Gets the product by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductDto> GetById(int id)
        {
            try
            {
                var data = await _productRepository.GetAsync(id);
                var productToReturn = ObjectMapper.Map<ProductDto>(data);
                productToReturn.ResponsibleGroupIds = _responsibleGroupRepository.GetAllList(p => p.ProductId == id).Select(pg => pg.GroupId).ToList();
                productToReturn.ProductSerials = _productSerialNoRepository.GetAllList(s => s.ProductId == data.Id).Select(s => s.Name).ToList();
                await GetProductCustomFieldsAndAssignToProductDto(id, productToReturn);
                return productToReturn;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private async Task GetProductCustomFieldsAndAssignToProductDto(int productId, ProductDto productToReturn)
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            //List<EntityFieldMappingDto> customerEntityCustomFields = await GetCustomerCustomFields(customerId);
            var customerEntityCustomFields = await _customFieldManager.GetObjectMappedCustomFields(productId, Screen.Product);
            var customerScreenCustomFields = await _customFieldManager.GetScreenCustomFields((int)Screen.Product);

            if (_customFieldManager.IsEntityHasNoCustomFieldsOrScreenHasNewCustomFields(customerEntityCustomFields, customerScreenCustomFields))
            {
                if (_customFieldManager.IsHasCustomFields(customerEntityCustomFields))
                {
                    var customerEntityCustomFieldIds = customerEntityCustomFields.Select(f => f.CustomFieldId);
                    var newCustomFields = customerScreenCustomFields.Where(f => customerEntityCustomFieldIds.Contains(f.CustomFieldId) == false).ToList();
                    _customFieldManager.InitializeFieldValueWithEmptyString(newCustomFields);
                    // await CreateEntityFieldMappings(newCustomFields, tenantId, customerId);
                    await _customFieldManager.CreateEntityFieldMappings(newCustomFields, tenantId, productId);
                }
                else if (_customFieldManager.IsHasCustomFields(customerScreenCustomFields))
                {
                    //InitializeFieldValueWithEmptyString(customerScreenCustomFields);
                    _customFieldManager.InitializeFieldValueWithEmptyString(customerScreenCustomFields);
                    // await CreateEntityFieldMappings(customerScreenCustomFields, tenantId, customerId);
                    await _customFieldManager.CreateEntityFieldMappings(customerScreenCustomFields, tenantId, productId);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                //customerToReturn.CustomFields = await GetCustomerCustomFields(customerId);
                productToReturn.CustomFields = await _customFieldManager.GetObjectMappedCustomFields(productId, Screen.Product);
            }
            else
                productToReturn.CustomFields = customerEntityCustomFields;
        }
        #endregion

        #region Update
        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task Update(ProductDto product)
        {
            var productDb = await _productRepository.GetAsync(product.Id);
            if (productDb == null)
            {
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.ActivityNotFound);
            }
            ObjectMapper.Map(product, productDb);

            await DeletePictureFromCloudeIfOldPicturePublicIdIsAvailable(productDb);
            await UploadPictureAndAsignPictureUrlAndPublicIdValueToProductIfBase64StringIsAvailable(product, productDb);

            await DeleteOldEntityFieldMappingsAsync(product);
            await _customFieldManager.CreateEntityFieldMappings(product.CustomFields, productDb.TenantId, product.Id);
        }

        private async Task DeletePictureFromCloudeIfOldPicturePublicIdIsAvailable(Product productDb)
        {
            if (!string.IsNullOrWhiteSpace(productDb.PicturePublicId))
                await _imageHelperService.DeleteImageAsync(productDb.PicturePublicId);
        }

        private async Task UploadPictureAndAsignPictureUrlAndPublicIdValueToProductIfBase64StringIsAvailable(ProductDto product, Product productDb)
        {
            var result = new MediaUploadDto();
            if (!string.IsNullOrWhiteSpace(product.Base64Picture))
                result = await _imageHelperService.AddMediaAsync(product.Base64Picture);
            productDb.PicturePublicId = result.PublicId;
            productDb.PictureUrl = result.Url;
        }

        private async Task DeleteOldEntityFieldMappingsAsync(ProductDto product)
        {
            //List<EntityFieldMapping> oldCustomFields = await GetOldEntityFieldMappingsAsync(customer);
            var oldCustomFields = await _customFieldManager.GetEntityFieldMappingsAsync(product.Id);

            //foreach (var field in oldCustomFields)
            //{
            //    await _entityFieldMappingRepository.DeleteAsync(field);
            //}

            await _customFieldManager.DeleteEntityFieldMappingsAsync(oldCustomFields);
        }


        #endregion


        /// <summary>
        ///  get all product 
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<ProductDto>> GetAll()
        {
            var products = await _productRepository.GetAll().Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.ProductNumber + " " + p.Name + (p.IsMedicalDevice ? "(Medical Device)" : string.Empty),
                SalesPrice = p.SalesPrice,
                CostPrice = p.CostPrice,
                ProductNumber = p.ProductNumber
            })
                  .ToListAsync();
            return new ListResultDto<ProductDto>(products);
        }


        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="product"></param>

        public async Task Delete(EntityDto product)
        {
            var data = await _productRepository.GetAsync(product.Id);
            if (data == null)
            {
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.ActivityNotFound);
            }

            await _productRepository.DeleteAsync(data);
        }


        private static Product GetProductActivityModel(ProductDto product, int tenantid, string pictureUrl, string picturePublicId)
        {

            return Product.Create(product.IsMedicalDevice, tenantid, product.ProductNumber, product.Name, product.Description, product.ProductGroupNumber,
                product.SalesPrice, product.CostPrice, product.Unit, product.Barcode, product.Access, product.Recprice, product.Category
                , product.Location, product.GrossWeight, product.Volume, product.ProductDiscountGroup, product.MinStock, product.MinOrder,
                product.RecCostPrice, product.InStock, pictureUrl, picturePublicId, product.CategoryId, product.BrandId, product.SupplierId, null);// TODO: Remove Employee Id from table, not needed now
        }


        /// <summary>
        ///  Gets the products with pagination.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductDto>> GetPagedResultAsync(PagedProductResultRequestDto input)
        {

            var query = _productRepository.GetAllIncluding(p => p.ResponsibleGroups);
            if (input.userId.HasValue && input.userId > 0)
            {
                query = await GetUserProducts(input.userId.Value, query);
            }
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(x => x.Name.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword)
                || x.CostPrice.ToString().Contains(input.Keyword)
                || x.SalesPrice.ToString().Contains(input.Keyword)
                || x.Unit.ToString().Contains(input.Keyword)
                || x.Barcode.Contains(input.Keyword)
                || x.Access.Contains(input.Keyword)
                || x.Recprice.Contains(input.Keyword)
                || x.Category.Contains(input.Keyword)
                || x.Location.Contains(input.Keyword)
                );
            }
            if (input.categoryId.HasValue && input.categoryId > 0)
                query = query.Where(p => p.CategoryId == input.categoryId);
            if (input.brandId.HasValue && input.brandId > 0)
                query = query.Where(p => p.BrandId == input.brandId);
            if (input.IsMedicalDevice.HasValue)
                query = query.Where(p => p.IsMedicalDevice == input.IsMedicalDevice);

            IQueryable<ProductDto> selectQuery = GetSelectQueryForProductList(query);
            var result = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            return result;
        }

        private async Task<IQueryable<Product>> GetUserProducts(long userId, IQueryable<Product> query)
        {
            var groupIds = await getUserGroups(userId);
            if (groupIds.Count > 0)
            {
                query = query.Where(p => p.ResponsibleGroups.Any(pg => groupIds.Contains(pg.GroupId)));
            }
            return query;
        }

        public async Task<List<long?>> getUserGroups(long? Id)
        {
            var query = _employeeGroupRepository.GetAll();
            query = query.Where(customerGroup => customerGroup.EmployeeId == Id);
            List<long?> groupIds = await query.Select(userGroup => (long?)userGroup.GroupId).ToListAsync();

            return groupIds;
        }

        private static IQueryable<ProductDto> GetSelectQueryForProductList(IQueryable<Product> query)
        {
            return query.Select(x => new ProductDto
            {

                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ProductNumber = x.ProductNumber,
                ProductGroupNumber = x.ProductGroupNumber,
                SalesPrice = x.SalesPrice,
                CostPrice = x.CostPrice,
                Unit = x.Unit,
                Barcode = x.Barcode,
                Access = x.Access,
                Recprice = x.Recprice,
                Category = x.Category,
                Location = x.Location,
                GrossWeight = x.GrossWeight,
                Volume = x.Volume,
                ProductDiscountGroup = x.ProductDiscountGroup,
                MinStock = x.MinStock,
                MinOrder = x.MinOrder,
                RecCostPrice = x.RecCostPrice,
                CreatorUserId = x.CreatorUserId,
                CreationTime = x.CreationTime,
                InStock = x.InStock,
                Base64Picture = x.PictureUrl,
                CategoryName = x.Category1 != null ? x.Category1.Name : "",
                BrandName = x.Brand != null ? x.Brand.Name : "",
                IsMedicalDevice = x.IsMedicalDevice,
                ProductSerials = x.ProductSerials.Select(s => s.Name).ToList(),
                Serials = x.ProductSerials.Where(p => p.IsSold == false).Select(s => new ProductSerial { Id = s.Id, Name = s.Name }).ToList()
            });
        }
    }
}
