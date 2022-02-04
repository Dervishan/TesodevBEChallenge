﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesodevBEChallenge.Api.Customers.Db
{
    public class CustomersDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public CustomersDbContext(DbContextOptions options) : base(options)
        {
        }
        
    }
}
