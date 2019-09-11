using sXb_service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.ViewModels
{
    public class ListingViewModel
    {
        public Guid Id { get; set; }

        public UserBook UserBook { get; set; }

        public Guid UserBookId { get; set; }

        public decimal Price { get; set; }

        public bool Sold { get; set; }

        public bool Deleted { get; set; }
    }
}
