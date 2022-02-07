using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TesodevBEChallenge.Api.Customers.Models
{
    public class Address
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string AddressLine { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public int CityCode { get; set; }
    }
}
