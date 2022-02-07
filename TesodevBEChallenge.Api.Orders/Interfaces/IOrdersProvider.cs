using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesodevBEChallenge.Api.Orders.Models;

namespace TesodevBEChallenge.Api.Orders.Interfaces
{
    public interface IOrdersProvider
    {
        /// <summary>
        /// Gets all of the orders
        /// </summary>
        /// <returns></returns>
        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync();
        /// <summary>
        /// Gets the order by the given id
        /// </summary>
        /// <param name="id">Integer order id</param>
        /// <returns></returns>
        Task<(bool IsSuccess, Order Order, string ErrorMessage)> GetOrderAsync(int orderId);
        Task<(bool IsSuccess, Order Order, string ErrorMessage)> CreateOrderAsync(Order order);
        Task<(bool IsSuccess, Order Order, string ErrorMessage)> UpdateOrderAsync(Order order);
        Task<(bool IsSuccess, string ErrorMessage)> ChangeStatusAsync(int id, string status);
        Task<(bool IsSuccess, string ErrorMessage)> DeleteOrderAsync(int id);
        /// <summary>
        /// Gets orders associated with the given customer id
        /// </summary>
        /// <param name="customerId">Integer customer id</param>
        /// <returns></returns>
        //Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersByCustomerIdAsync(int customerId);

    }
}
