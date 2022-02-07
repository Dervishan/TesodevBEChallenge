using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TesodevBEChallenge.Api.Customers.Db;
using TesodevBEChallenge.Api.Customers.Profiles;
using TesodevBEChallenge.Api.Customers.Providers;
using Xunit;

namespace TesodevBEChallenge.Api.Tests
{
    public class CustomersServiceTest
    {
        [Fact]
        public async Task GetCustomersReturnAllCustomers()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>().UseInMemoryDatabase(nameof(GetCustomersReturnAllCustomers)).Options;
            var customersDbContext = new CustomersDbContext(options);
            CreateCustomers(customersDbContext);
            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);
            var customersProvider = new CustomersProvider(customersDbContext, null, mapper);
            var customer = await customersProvider.GetCustomersAsync();
            Assert.True(customer.IsSuccess);
            Assert.True(customer.Customers.Any());
            Assert.Null(customer.ErrorMessage);
        }

        [Fact]
        public async Task GetCustomerReturnsCustomerUsingValidId()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>().UseInMemoryDatabase(nameof(GetCustomersReturnAllCustomers)).Options;
            var customersDbContext = new CustomersDbContext(options);
            CreateCustomers(customersDbContext);
            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);
            var customersProvider = new CustomersProvider(customersDbContext, null, mapper);
            var customer = await customersProvider.GetCustomerAsync(1);
            Assert.True(customer.IsSuccess);
            Assert.NotNull(customer.Customer);
            Assert.True(customer.Customer.Id == 1);
            Assert.Null(customer.ErrorMessage);
        }

        [Fact]
        public async Task GetCustomerReturnsCustomerUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>().UseInMemoryDatabase(nameof(GetCustomersReturnAllCustomers)).Options;
            var customersDbContext = new CustomersDbContext(options);
            CreateCustomers(customersDbContext);
            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);
            var customersProvider = new CustomersProvider(customersDbContext, null, mapper);
            var customer = await customersProvider.GetCustomerAsync(-1);
            Assert.False(customer.IsSuccess);
            Assert.Null(customer.Customer);
            Assert.NotNull(customer.ErrorMessage);
        }

        [Fact]
        public async Task CreateCustomerReturnsCustomerUsingValidModel()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>().UseInMemoryDatabase(nameof(GetCustomersReturnAllCustomers)).Options;
            var customersDbContext = new CustomersDbContext(options);
            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);
            var customersProvider = new CustomersProvider(customersDbContext, null, mapper);
;
            var newCustomer = new Customers.Models.Customer()
            {
                Id = 1,
                Name = "Dervishan",
                Email = "dervishan.sezer@gmail.com",
                Address = new Customers.Models.Address() { AddressLine = Guid.NewGuid().ToString() + " Street", City = Guid.NewGuid().ToString(), Country = Guid.NewGuid().ToString(), CityCode = new Random().Next(1111, 9999) },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var customer = await customersProvider.CreateCustomerAsync(newCustomer);
            Assert.True(customer.IsSuccess);
            Assert.NotNull(customer.Customer);
            Assert.True(customer.Customer.Id == 1);
            Assert.Null(customer.ErrorMessage);
        }

        [Fact]
        public async Task CreateCustomerReturnsCustomerUsingInvalidModel()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>().UseInMemoryDatabase(nameof(GetCustomersReturnAllCustomers)).Options;
            var customersDbContext = new CustomersDbContext(options);
            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);
            var customersProvider = new CustomersProvider(customersDbContext, null, mapper);
            ;
            var newCustomer = new Customers.Models.Customer()
            {
                Id = 1,
                Email = "dervishan.sezer@gmail.com",
                Address = new Customers.Models.Address() { AddressLine = Guid.NewGuid().ToString() + " Street", City = Guid.NewGuid().ToString(), Country = Guid.NewGuid().ToString(), CityCode = new Random().Next(1111, 9999) },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var customer = await customersProvider.CreateCustomerAsync(newCustomer);
            Assert.False(customer.IsSuccess);
            Assert.Null(customer.Customer);
            Assert.NotNull(customer.ErrorMessage);
        }

        [Fact]
        public async Task UpdateCustomerReturnsCustomerUsingValidModel()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>().UseInMemoryDatabase(nameof(GetCustomersReturnAllCustomers)).Options;
            var customersDbContext = new CustomersDbContext(options);
            CreateCustomers(customersDbContext);
            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);
            var customersProvider = new CustomersProvider(customersDbContext, null, mapper);
            var customer = await customersProvider.GetCustomerAsync(1);
            var newCustomer = await customersProvider.UpdateCustomerAsync(customer.Customer);
            Assert.True(newCustomer.IsSuccess);
            Assert.NotNull(newCustomer.Customer);
            Assert.True(newCustomer.Customer.Id == 1);
            Assert.Null(newCustomer.ErrorMessage);
        }

        [Fact]
        public async Task UpdateCustomerReturnsCustomerUsingInvalidModel()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>().UseInMemoryDatabase(nameof(GetCustomersReturnAllCustomers)).Options;
            var customersDbContext = new CustomersDbContext(options);
            CreateCustomers(customersDbContext);
            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);
            var customersProvider = new CustomersProvider(customersDbContext, null, mapper);
            var customer = await customersProvider.GetCustomerAsync(-1);
            var newCustomer = await customersProvider.UpdateCustomerAsync(customer.Customer);
            Assert.False(newCustomer.IsSuccess);
            Assert.Null(newCustomer.Customer);
            Assert.NotNull(newCustomer.ErrorMessage);
        }

        /// <summary>
        /// Creates 10 random customers
        /// </summary>
        /// <param name="customersDbContext"></param>
        private void CreateCustomers(CustomersDbContext customersDbContext)
        {
            string CustomerName;
            for (int i = 1; i <= 10; i++)
            {
                CustomerName = Guid.NewGuid().ToString();
                customersDbContext.Customers.Add(new Customer()
                {
                    Id = i,
                    Name = CustomerName,
                    Email = $"{CustomerName}@gmail.com",
                    Address = new Address() { AddressLine = Guid.NewGuid().ToString() + " Street", City = Guid.NewGuid().ToString(), Country = Guid.NewGuid().ToString(), CityCode = new Random().Next(1111, 9999) },
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            customersDbContext.SaveChanges();
        }
    }
}
