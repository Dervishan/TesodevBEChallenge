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
            throw new NotImplementedException();
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await ordersDbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Product)
                    .ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>,
                        IEnumerable<Models.Order>>(orders);
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
    }
}
