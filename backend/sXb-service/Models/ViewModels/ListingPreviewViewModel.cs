using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace sXb_service.Models.ViewModels
{
    public class ListingPreviewViewModel
    {
        public Guid Id { get; set; }

        [StringLength(265, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 256 characters")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "isbn10")]
        public string ISBN10 { get; set; }

        [JsonProperty(PropertyName = "isbn13")]
        public string ISBN13 { get; set; }

        [RangeAttribute(typeof(decimal), "0", "9223372036854775807", ErrorMessage = "Price must be above zero")]
        public decimal Price { get; set; }

        public string Condition { get; set; }

        public string Authors { get; set; }

        public string Thumbnail { get; set; }
    }
}