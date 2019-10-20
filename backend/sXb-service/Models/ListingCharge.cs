using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models
{
    public class ListingCharge
    {
        public Guid  Id { get; set; }

        public Guid ListingId { get; set; }

        public float Amount { get; set; }

        public string BuyerToken { get; set; }
    }
}
