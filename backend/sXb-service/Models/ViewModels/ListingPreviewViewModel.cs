using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models.ViewModels
{
    public class ListingPreviewViewModel
    {
        public Guid Id { get; set; }

        [StringLength(265, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 256 characters")]
        public string Title { get; set; }
        public string ISBN10 { get; set; }

        [RangeAttribute(typeof(decimal), "0", "9223372036854775807", ErrorMessage = "Price must be above zero")]
        public decimal Price { get; set; }

        public string Condition { get; set; }

        public List<string> Authors { get; set; }

        public Status Status { get; set; }
    }
}