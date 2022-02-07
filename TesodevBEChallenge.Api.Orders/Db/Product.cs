using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TesodevBEChallenge.Api.Orders.Db
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
