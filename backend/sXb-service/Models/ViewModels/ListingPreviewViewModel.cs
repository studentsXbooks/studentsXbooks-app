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

        [StringLength(265, MinimumLength = 1, ErrorMessage = "not working {2}")]
        public string Title { get; set; }

        [RangeAttribute(typeof(decimal), "0", "9223372036854775807")]
        public decimal Price { get; set; }

        public Condition Condition { get; set; }

        public List<string> Authors { get; set; }
    }
}
