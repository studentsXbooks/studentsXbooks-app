using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models
{
    public class Listing
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(UserBookId))]
        public UserBook UserBook { get; set; }

        public Guid UserBookId { get; set; }

        public decimal Price { get; set; }
        
        public bool Sold { get; set; }

        public bool Deleted { get; set; }
    }
}
