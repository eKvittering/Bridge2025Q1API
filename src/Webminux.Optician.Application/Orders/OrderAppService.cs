using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician.CommonDtos;
using Webminux.Optician.Helpers;
using Webminux.Optician.Orders.Dto;

namespace Webminux.Optician.Orders
{
    /// <summary>
    /// Provides Create,Get and Update Order
    public class OrderAppService : OpticianAppServiceBase, IOrderAppService
    {
        #region Private Fields
        private readonly IOrderManager _orderManager;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrderAppService(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }
        #endregion

        #region Create Order
        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="input">Contain Order Basic information and List of Order Lines</param>
        public Task CreateAsync(CreateOrderDto input)
        {
            ValidateOrderLines(input);
            List<OrderLine> orderLines = GetOrderLinesFromCreateOrderDto(input);
            return GetOrderFromCreateOrderDtoAndCreateAsync(input, orderLines);
        }

        private Task GetOrderFromCreateOrderDtoAndCreateAsync(CreateOrderDto input, List<OrderLine> orderLines)
        {
            var order = Order.Create(GetTenantId(), input.OrderNumber, DateTime.ParseExact(input.OrderDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), input.Note, input.Received, input.EmployeeId, input.SupplierId, orderLines);
            return _orderManager.CreateAsync(order);
        }

        private List<OrderLine> GetOrderLinesFromCreateOrderDto(CreateOrderDto input)
        {
            var orderLines = new List<OrderLine>();
            foreach (var orderLine in input.OrderLines)
            {
                var orderLineDb = OrderLine.Create(GetTenantId(), DateTime.ParseExact(orderLine.PromissedDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), orderLine.Quantity, orderLine.Price, orderLine.ProductId);
                orderLines.Add(orderLineDb);
            }

            return orderLines;
        }

        private static void ValidateOrderLines(CreateOrderDto input)
        {
            if (input.OrderLines.Count() == 0)
            {
                throw new UserFriendlyException("Order Lines cannot be empty!");
            }
        }
        #endregion

        #region Get Paged Result
        /// <summary>
        /// Get All Orders with pagination
        /// </summary>
        public async Task<PagedResultDto<OrderDto>> GetAllAsync(PagedOrderResultRequestDto input)
        {
            var query = _orderManager.GetAll();
            query = ApplyFilters(input, query);
            IQueryable<OrderDto> selectQuery = GetSelectQuery(query);
            return await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
        }


        private static IQueryable<Order> ApplyFilters(PagedOrderResultRequestDto input, IQueryable<Order> query)
        {
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(x => x.OrderNumber.Contains(input.Keyword));
            }

            return query;
        }
        #endregion

        #region Get Order
        /// <summary>
        /// Get Order by Id
        /// </summary>
        public async Task<OrderDto> GetAsync(EntityDto input)
        {
            var query = _orderManager.GetAll();
            var order = await GetSelectQuery(query).FirstOrDefaultAsync(x => x.Id == input.Id);
            return order;
        }
        #endregion

        #region Update Order
        /// <summary>
        /// Update Order and Delete Old Order Lines
        /// </summary>
        /// <param name="input">Contain Order Basic information and List of Order Lines</param>
        public async Task UpdateAsync(UpdateOrderDto input)
        {
            var order = ObjectMapper.Map<Order>(input);
            await _orderManager.UpdateAsync(order);
        }
        #endregion

        #region Get Next Order Number
        /// <summary>
        /// Provide Next Order Number in for create order form
        /// </summary>
        /// <returns>Order Number of new entity</returns>
        public async Task<SingleValueDto<string>> GetNextOrderNumber()
        {
            var ordersCount = await _orderManager.GetAll().CountAsync();
            var nextOrderNumber = ordersCount + 1;
            return new SingleValueDto<string>(nextOrderNumber.ToString());
        }
        #endregion

        #region Mark Order As Received

        /// <summary>
        /// Confirms that order is received
        /// </summary>
        /// <param name="input">Order Id</param>
        /// <returns></returns>
        public async Task MarkOrderAsReceived(EntityDto input)
        {
            var order =await _orderManager.GetAll().FirstOrDefaultAsync(order => order.Id == input.Id);
            if (order == null)
                throw new UserFriendlyException("Oder Id " + input.Id + " not found");
            order.Received = true;
        }
        #endregion
      
        #region Helpers
        private static IQueryable<OrderDto> GetSelectQuery(IQueryable<Order> query)
        {
            return query.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                Note = order.Note,
                Received = order.Received,
                EmployeeId = order.EmployeeId,
                EmployeeName = order.Employee.FullName,
                SupplierId = order.SupplierId,
                SupplierName = order.Supplier.User.FullName,
                CreationTime = order.CreationTime,
                CreatorUserId = order.CreatorUserId,
                OrderLines = order.OrderLines.Select(orderLine => new OrderLineListDto
                {
                    Id = orderLine.Id,
                    PromissedDate = orderLine.PromissedDate,
                    Quantity = orderLine.Quantity,
                    Price = orderLine.Price,
                    ProductId = orderLine.ProductId,
                    ProductName = orderLine.Product.Name
                }).ToList()
            });
        }

        #endregion
    }
}
