using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesodevBEChallenge.Api.Orders.Db;
using TesodevBEChallenge.Api.Orders.Interfaces;

namespace TesodevBEChallenge.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext ordersDbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext ordersDbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.ordersDbContext = ordersDbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!ordersDbContext.Orders.Any())
            {
                ordersDbContext.Orders.Add(new Db.Order() { 
                    Id = 1, 
                    CustomerId = 1, 
                    Quantity = 23, 
                    Price = 10, 
                    Status = "Approval", 
                    AddressId = 2, 
                    Product = new Product() { Id = 1, ImageUrl = "www.link.com", Name = "Gazoz" }, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                });
                ordersDbContext.Orders.Add(new Db.Order() { 
                    Id = 2, 
                    CustomerId = 2, 
                    Quantity = 15, 
                    Price = 6, 
                    Status = "Approved", 
                    AddressId = 2, 
                    Product = new Product() { Id = 3, ImageUrl = "www.link.com", Name = "Meyve Suyu" }, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                });
                ordersDbContext.Orders.Add(new Db.Order() { 
                    Id = 3, 
                    CustomerId = 2, 
                    Quantity = 126, 
                    Price = 2, 
                    Status = "Delivered", 
                    AddressId = 2, 
                    Product = new Product() { Id = 2, ImageUrl = "www.link.com", Name = "Ayran" }, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                });
                ordersDbContext.SaveChanges();
            }
        }
        /// <summary>
        /// Gets all of the orders
        /// </summary>
        /// <returns>Order List</returns>
        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync()
        {
            try
            {
                var orders = await ordersDbContext.Orders.Include(o => o.Product).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        /// <summary>
        /// Gets the order by the given id
        /// </summary>
        /// <param name="id">Integer order id</param>
        /// <returns>Order</returns>
        public async Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> GetOrderAsync(int id)
        {
            try
            {
                logger?.LogInformation("Querying orders");
                var order = await ordersDbContext.Orders.Include(o => o.Product).FirstOrDefaultAsync(o => o.Id == id);
                if (order != null)
                {
                    logger?.LogInformation("Order found");
                    var result = mapper.Map<Db.Order, Models.Order>(order);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Creates an Order
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Order</returns>
        public async Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> CreateOrderAsync(Models.Order order)
        {
            try
            {
                var orderCount = await ordersDbContext.Orders.LongCountAsync();
                if (order.Quantity != 0 && order.Price != 0 && order.Status != null)
                {
                    var newOrder = new Db.Order()
                    {
                        Id = (int)(orderCount + 1),
                        CustomerId = order.CustomerId,
                        Quantity =order.Quantity,
                        Price = order.Price,
                        Status = order.Status,
                        AddressId = order.AddressId,
                        Product = new Db.Product() { Id = order.Product.Id, ImageUrl = order.Product.ImageUrl, Name = order.Product.Name },
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    logger?.LogInformation("Order created");
                    ordersDbContext.Orders.Add(newOrder);
                    ordersDbContext.SaveChanges();
                    var result = mapper.Map<Db.Order, Models.Order>(newOrder);
                    return (true, result, null);
                }
                return (false, null, "One input is empty");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        /// <summary>
        /// Updates an Order
        /// </summary>
        /// <param name="newOrder">Updated Order</param>
        /// <returns>Order</returns>
        public async Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> UpdateOrderAsync(Models.Order newOrder)
        {
            try
            {
                logger?.LogInformation("Querying Orders");
                var order = await ordersDbContext.Orders.FindAsync(newOrder.Id);
                if (order != null)
                {
                    logger?.LogInformation("Order found");
                    if (newOrder.Quantity != 0)
                    {
                        order.Quantity = newOrder.Quantity;
                    }
                    if (newOrder.AddressId != 0)
                    {
                        order.AddressId = newOrder.AddressId;
                    }
                    if (newOrder.Product != null)
                    {
                        order.Product = new Db.Product() { Id = newOrder.Product.Id, ImageUrl = newOrder.Product.ImageUrl, Name = newOrder.Product.Name };
                    }
                    order.UpdatedAt = DateTime.UtcNow;
                    ordersDbContext.SaveChanges();
                    var result = mapper.Map<Db.Order, Models.Order>(order);
                    return (true, result, null);
                }
                return (false, null, "Order not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        /// <summary>
        /// Deletes a Order by given id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>bool</returns>
        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteOrderAsync(int id)
        {
            try
            {
                logger?.LogInformation("Finding Order");
                var order = await ordersDbContext.Orders.FindAsync(id);
                if (order != null)
                {
                    logger?.LogInformation("Order found");
                    var result = ordersDbContext.Orders.Remove(order);
                    await ordersDbContext.SaveChangesAsync();
                    return (true, null);
                }
                return (false, "Order not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, ex.Message);
            }
        }
        /// <summary>
        /// Changes an Orders Status
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="status">Order Status</param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string ErrorMessage)> ChangeStatusAsync(int id, string status)
        {
            try
            {
                logger?.LogInformation("Querying Orders");
                var order = await ordersDbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order != null)
                {
                    order.Status = status;
                    ordersDbContext.SaveChanges();
                    return (true, null);
                }
                return (false, "Order not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Gets orders associated with the given customer id
        /// </summary>
        /// <param name="customerId">Integer customer id</param>
        /// <returns>Order List</returns>
        //public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersByCustomerIdAsync(int customerId)
        //{
        //    try
        //    {
        //        var orders = await ordersDbContext.Orders
        //            .Where(o => o.CustomerId == customerId)
        //            .Include(o => o.Product)
        //            .ToListAsync();
        //        if (orders != null && orders.Any())
        //        {
        //            var result = mapper.Map<IEnumerable<Db.Order>,
        //                IEnumerable<Models.Order>>(orders);
        //            return (true, result, null);
        //        }
        //        return (false, null, "Not Found");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger?.LogError(ex.ToString());
        //        return (false, null, ex.Message);
        //    }
        //}
    }
}
