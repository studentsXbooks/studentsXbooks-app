using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models.ViewModels
{
    public class ListingDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Author Author { get; set; }

        public decimal Price { get; set; }
        
        public string ISBN { get; set; }

        public Condition Condition { get; set; }
    }
}
