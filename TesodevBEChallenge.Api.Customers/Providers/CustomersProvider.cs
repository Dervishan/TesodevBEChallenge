using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesodevBEChallenge.Api.Customers.Db;
using TesodevBEChallenge.Api.Customers.Interfaces;
using TesodevBEChallenge.Api.Customers.Models;

namespace TesodevBEChallenge.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { 
                    Id = 1, 
                    Name = "Dervishan", 
                    Email = "dervishan.sezer@gmail.com", 
                    Address = new Db.Address() { AddressLine = "Seyit Onbaşı Caddesi", City = "İstanbul", Country = "Türkiye", CityCode = 90216 }, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                });
                dbContext.Customers.Add(new Db.Customer() { 
                    Id = 2, 
                    Name = "Tim", 
                    Email = "tim.donar@gmail.com",
                    Address = new Db.Address() { AddressLine = "Gul Sokak", City = "Istanbul", Country = "Turkey", CityCode = 90212 }, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                });
                dbContext.Customers.Add(new Db.Customer() { 
                    Id = 3, 
                    Name = "Lina", 
                    Email = "lina.gheorge@gmail.com",
                    Address = new Db.Address() { AddressLine = "Schuberstraße", City = "Frankfurt", Country = "Germany", CityCode = 4969 }, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.Include(a => a.Address).ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
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

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                logger?.LogInformation("Querying customers");
                var customer = await dbContext.Customers.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
                if (customer != null)
                {
                    logger?.LogInformation("Customer found");
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
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
