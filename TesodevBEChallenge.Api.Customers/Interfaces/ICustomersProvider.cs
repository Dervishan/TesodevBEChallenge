using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesodevBEChallenge.Api.Customers.Models;

namespace TesodevBEChallenge.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsync();
        Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> GetCustomerAsync(int id);
        Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> CreateCustomerAsync(Customer customer);
        Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> UpdateCustomerAsync(Customer customer);
        Task<(bool IsSuccess, string ErrorMessage)> DeleteCustomerAsync(int id);
    }
}
