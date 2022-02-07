using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesodevBEChallenge.Api.Orders.Interfaces;

namespace TesodevBEChallenge.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            this.ordersProvider = ordersProvider;
        }
        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>Order List</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var result = await ordersProvider.GetOrdersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }
        /// <summary>
        /// Gets an Order by the given id
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Order</returns>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderAsync(int orderId)
        {
            var result = await ordersProvider.GetOrderAsync(orderId);
            if (result.IsSuccess)
            {
                return Ok(result.Order);
            }
            return NotFound();
        }

        //[HttpGet("{customerId}")]

        //public async Task<IActionResult> GetOrdersByCustomerIdAsync(int customerId)
        //{
        //    var result = await ordersProvider.GetOrdersByCustomerIdAsync(customerId);
        //    if (result.IsSuccess)
        //    {
        //        return Ok(result.Orders);
        //    }
        //    return NotFound();
        //}


        /// <summary>
        /// Creates Order
        /// </summary>
        /// <returns>Order</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Models.Order order)
        {
            var result = await ordersProvider.CreateOrderAsync(order);
            if (result.IsSuccess)
            {
                return Ok(result.Order);
            }
            return NotFound();
        }

        /// <summary>
        /// Updates Order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Order</returns>
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(int orderId, Models.Order order)
        {
            if (orderId != order.Id)
            {
                return BadRequest();
            }
            var result = await ordersProvider.UpdateOrderAsync(order);
            if (result.IsSuccess)
            {
                return Ok(result.Order);
            }
            return NotFound();
        }

        /// <summary>
        /// Deletes Order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Bool</returns>
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await ordersProvider.DeleteOrderAsync(orderId);
            if (result.IsSuccess)
            {
                return Ok(RedirectToAction(nameof(GetOrdersAsync)));
            }
            return NotFound();
        }

    }
}
