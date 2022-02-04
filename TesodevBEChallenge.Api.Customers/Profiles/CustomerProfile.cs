using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesodevBEChallenge.Api.Customers.Profiles
{
    public class CustomerProfile : AutoMapper.Profile
    {
        public CustomerProfile()
        {
            CreateMap<Db.Customer, Models.Customer>();
            CreateMap<Db.Address, Models.Address>();
        }
    }
}
