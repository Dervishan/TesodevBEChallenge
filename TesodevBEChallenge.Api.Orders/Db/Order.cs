using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TesodevBEChallenge.Api.Orders.Db
{
    public class Order
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public int AddressId { get; set; }
        [Required]
        public Product Product { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
