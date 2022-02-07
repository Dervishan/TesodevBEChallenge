using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesodevBEChallenge.Api.Customers.Interfaces;

namespace TesodevBEChallenge.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersProvider customersProvider;

        public CustomersController(ICustomersProvider customersProvider)
        {
            this.customersProvider = customersProvider;
        }

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <returns>Customer List</returns>
        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await customersProvider.GetCustomersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Customers);
            }
            return NotFound();
        }

        /// <summary>
        /// Gets Customer by an ID
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var result = await customersProvider.GetCustomerAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Customer);
            }
            return NotFound();
        }

        /// <summary>
        /// Creates Customer
        /// </summary>
        /// <returns>Customer</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                var result = await customersProvider.CreateCustomerAsync(customer);
                if (result.IsSuccess)
                {
                    return Ok(result.Customer);
                }
                return NotFound();
            }
            return ValidationProblem();
        }

        /// <summary>
        /// Updates Customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Customer</returns>
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCustomer(int id, Models.Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var result = await customersProvider.UpdateCustomerAsync(customer);
                if (result.IsSuccess)
                {
                    return Ok(result.Customer);
                }
                return NotFound();
            }
            return ValidationProblem();
        }

        /// <summary>
        /// Deletes Customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Bool</returns>
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await customersProvider.DeleteCustomerAsync(id);
            if (result.IsSuccess)
            {
                return Ok(RedirectToAction(nameof(GetCustomersAsync)));
            }
            return NotFound();
        }
    }
}
