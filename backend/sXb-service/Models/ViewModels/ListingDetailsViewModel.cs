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

        public Guid UserBookId { get; set; }

        public decimal Price { get; set; }

        public bool Sold { get; set; }

        public bool Deleted { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public string ImageURL { get; set; }
    }
}
