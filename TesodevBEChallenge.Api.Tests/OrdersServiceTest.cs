using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TesodevBEChallenge.Api.Orders.Db;
using TesodevBEChallenge.Api.Orders.Profiles;
using TesodevBEChallenge.Api.Orders.Providers;
using Xunit;

namespace TesodevBEChallenge.Api.Tests
{
    public class OrdersServiceTest
    {
        [Fact]
        public async Task GetOrdersReturnAllOrders()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(GetOrdersReturnAllOrders)).Options;
            var ordersDbContext = new OrdersDbContext(options);
            CreateOrders(ordersDbContext);
            var orderProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(orderProfile));
            var mapper = new Mapper(configuration);
            var ordersProvider = new OrdersProvider(ordersDbContext, null, mapper);
            var order = await ordersProvider.GetOrdersAsync();
            Assert.True(order.IsSuccess);
            Assert.True(order.Orders.Any());
            Assert.Null(order.ErrorMessage);
        }

        [Fact]
        public async Task GetOrderReturnsOrderUsingValidId()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(GetOrdersReturnAllOrders)).Options;
            var OrdersDbContext = new OrdersDbContext(options);
            CreateOrders(OrdersDbContext);
            var OrderProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(OrderProfile));
            var mapper = new Mapper(configuration);
            var OrdersProvider = new OrdersProvider(OrdersDbContext, null, mapper);
            var Order = await OrdersProvider.GetOrderAsync(1);
            Assert.True(Order.IsSuccess);
            Assert.NotNull(Order.Order);
            Assert.True(Order.Order.Id == 1);
            Assert.Null(Order.ErrorMessage);
        }

        [Fact]
        public async Task GetOrderReturnsOrderUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(GetOrdersReturnAllOrders)).Options;
            var OrdersDbContext = new OrdersDbContext(options);
            CreateOrders(OrdersDbContext);
            var OrderProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(OrderProfile));
            var mapper = new Mapper(configuration);
            var OrdersProvider = new OrdersProvider(OrdersDbContext, null, mapper);
            var Order = await OrdersProvider.GetOrderAsync(-1);
            Assert.False(Order.IsSuccess);
            Assert.Null(Order.Order);
            Assert.NotNull(Order.ErrorMessage);
        }

        [Fact]
        public async Task CreateOrderReturnsOrderUsingValidModel()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(GetOrdersReturnAllOrders)).Options;
            var OrdersDbContext = new OrdersDbContext(options);
            var OrderProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(OrderProfile));
            var mapper = new Mapper(configuration);
            var OrdersProvider = new OrdersProvider(OrdersDbContext, null, mapper);
;
            var newOrder = new Orders.Models.Order()
            {
                Id = 1,
                CustomerId = 1,
                Quantity = 235,
                Price = 15,
                Status = "Trial",
                AddressId = 1,
                Product = new Orders.Models.Product() { Id = 1, ImageUrl = "url", Name = "name" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var Order = await OrdersProvider.CreateOrderAsync(newOrder);
            Assert.True(Order.IsSuccess);
            Assert.NotNull(Order.Order);
            Assert.True(Order.Order.Id == 1);
            Assert.Null(Order.ErrorMessage);
        }

        [Fact]
        public async Task CreateOrderReturnsOrderUsingInvalidModel()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(GetOrdersReturnAllOrders)).Options;
            var OrdersDbContext = new OrdersDbContext(options);
            var OrderProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(OrderProfile));
            var mapper = new Mapper(configuration);
            var OrdersProvider = new OrdersProvider(OrdersDbContext, null, mapper);
            ;
            var newOrder = new Orders.Models.Order()
            {
                Id = 1,
                Quantity = 235,
                Price = 15,
                Status = "Trial",
                AddressId = 1,
                Product = new Orders.Models.Product() { Id = 1, ImageUrl = "url", Name = "name" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var Order = await OrdersProvider.CreateOrderAsync(newOrder);
            Assert.False(Order.IsSuccess);
            Assert.Null(Order.Order);
            Assert.NotNull(Order.ErrorMessage);
        }

        [Fact]
        public async Task UpdateOrderReturnsOrderUsingValidModel()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(GetOrdersReturnAllOrders)).Options;
            var OrdersDbContext = new OrdersDbContext(options);
            CreateOrders(OrdersDbContext);
            var OrderProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(OrderProfile));
            var mapper = new Mapper(configuration);
            var OrdersProvider = new OrdersProvider(OrdersDbContext, null, mapper);
            var Order = await OrdersProvider.GetOrderAsync(1);
            var newOrder = await OrdersProvider.UpdateOrderAsync(Order.Order);
            Assert.True(newOrder.IsSuccess);
            Assert.NotNull(newOrder.Order);
            Assert.True(newOrder.Order.Id == 1);
            Assert.Null(newOrder.ErrorMessage);
        }

        [Fact]
        public async Task UpdateOrderReturnsOrderUsingInvalidModel()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(GetOrdersReturnAllOrders)).Options;
            var OrdersDbContext = new OrdersDbContext(options);
            CreateOrders(OrdersDbContext);
            var OrderProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(OrderProfile));
            var mapper = new Mapper(configuration);
            var OrdersProvider = new OrdersProvider(OrdersDbContext, null, mapper);
            var Order = await OrdersProvider.GetOrderAsync(-1);
            var newOrder = await OrdersProvider.UpdateOrderAsync(Order.Order);
            Assert.False(newOrder.IsSuccess);
            Assert.Null(newOrder.Order);
            Assert.NotNull(newOrder.ErrorMessage);
        }

        /// <summary>
        /// Creates 10 random Orders
        /// </summary>
        /// <param name="OrdersDbContext"></param>
        private void CreateOrders(OrdersDbContext ordersDbContext)
        {
            string productName;
            for (int i = 1; i <= 10; i++)
            {
                productName = Guid.NewGuid().ToString();
                ordersDbContext.Orders.Add(new Order()
                {
                    Id = i,
                    CustomerId = 1,
                    Quantity = i + 235,
                    Price = (double)(i + 15),
                    Status = "Trial",
                    AddressId = 1,
                    Product = new Product() { ImageUrl = "url", Name = productName },
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            ordersDbContext.SaveChanges();
        }
    }
}
