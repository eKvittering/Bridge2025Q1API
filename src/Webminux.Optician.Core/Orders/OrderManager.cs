using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<OrderLine> _orderLineRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public OrderManager(
            IRepository<Order> orderRepository,
            IRepository<Product> productRepository,
            IRepository<OrderLine> orderLineRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderLineRepository = orderLineRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task CreateAsync(Order order)
        {
            await _orderRepository.InsertAsync(order);
            await UpdateProductStock(order);
        }


        public IQueryable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public async Task UpdateAsync(Order order)
        {
            await UpdateOrderLines(order);
            await UpdateOrder(order);
            await UpdateProductStock(order);
        }

        #region Helpers
        private async Task UpdateOrder(Order order)
        {
            order.OrderLines = null;
            await _orderRepository.UpdateAsync(order);
        }

        private async Task AddNewOrderLines(ICollection<OrderLine> orderLines)
        {
            foreach (var orderLine in orderLines)
            {
                await _orderLineRepository.InsertAsync(orderLine);
            }
        }

        private async Task UpdateOrderLines(Order order)
        {
            List<OrderLine> oldOrderLines = await GetOldOrderLines(order);
            List<int> oldOrderLinesIds = GetOrderLineIds(oldOrderLines);
            await FindOrderLinedNeedToBeAddedAndAdd(order);
            UpdateExistingOrderLines(order, oldOrderLines, oldOrderLinesIds);
            await DeleteRemovedOrderLines(order, oldOrderLines);
            await _unitOfWorkManager.Current.SaveChangesAsync();
        }

        private static List<int> GetOrderLineIds(List<OrderLine> oldOrderLines)
        {
            return oldOrderLines.Select(orderLine => orderLine.Id).ToList();
        }

        private async Task<List<OrderLine>> GetOldOrderLines(Order order)
        {
            return await _orderLineRepository.GetAllListAsync(orderLine => orderLine.OrderId == order.Id);
        }

        private async Task DeleteRemovedOrderLines(Order order, List<OrderLine> oldOrderLines)
        {
            var orderLinesNeedToBeRemoved = oldOrderLines.Where(orderLine => order.OrderLines.Select(ol => ol.Id).Contains(orderLine.Id) == false);
            foreach (var orderLine in orderLinesNeedToBeRemoved)
            {
                await ResetProductStockAndDeleteOrderLine(orderLine);
            }
        }

        private async Task ResetProductStockAndDeleteOrderLine(OrderLine orderLine)
        {
            await ResetDeletedProductStock(orderLine.ProductId, orderLine.Quantity);
            await _orderLineRepository.DeleteAsync(orderLine.Id);
        }

        private void UpdateExistingOrderLines(Order order, List<OrderLine> oldOrderLines, List<int> oldOrderLinesIds)
        {
            var orderLinesNeedToBeUpdated = order.OrderLines.Where(orderLine => oldOrderLinesIds.Contains(orderLine.Id));
            foreach (var orderLine in orderLinesNeedToBeUpdated)
            {
                UpdateInvoiceLine(oldOrderLines, orderLine);
            }
        }

        private static void UpdateInvoiceLine(List<OrderLine> oldOrderLines, OrderLine orderLine)
        {
            var orderLineFromDb = oldOrderLines.First(ol => ol.Id == orderLine.Id);
            orderLineFromDb.Price = orderLine.Price;
            orderLineFromDb.ProductId = orderLine.ProductId;
            orderLineFromDb.PromissedDate = orderLine.PromissedDate;
            orderLineFromDb.Quantity = orderLine.Quantity;
        }

        private async Task FindOrderLinedNeedToBeAddedAndAdd(Order order)
        {
            var orderLinesNeedToBeAdded = order.OrderLines.Where(orderLine => orderLine.Id == 0).ToList();
            await AddNewOrderLines(orderLinesNeedToBeAdded);
        }

        private async Task ResetDeletedProductStock(int productId, int quantity)
        {
            var product = await _productRepository.FirstOrDefaultAsync(productId);
            product.InStock -= quantity;
        }


        private async Task UpdateProductStock(Order order)
        {
            foreach (var orderLine in order.OrderLines)
            {
                await GetProductAndUpdateQuantity(orderLine);
            }
        }

        private async Task GetProductAndUpdateQuantity(OrderLine orderLine)
        {
            var product = await _productRepository.FirstOrDefaultAsync(orderLine.ProductId);
            ValidateProduct(product);
            product.InStock += orderLine.Quantity;
        }

        private static void ValidateProduct(Product product)
        {
            if (product == null)
            {
                throw new UserFriendlyException("Invalid Product Id");
            }
        }
    }
    #endregion
}
