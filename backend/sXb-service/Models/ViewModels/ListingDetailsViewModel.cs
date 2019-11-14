using Newtonsoft.Json;
using sXb_service.Helpers.ModelValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sXb_service.Models.ViewModels
{
    public class ListingDetailsViewModel
    {
        public Guid Id { get; set; }

        [StringLength(265, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 256 characters")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "isbn10")]
        [ISBNValidation(ISBNType = ISBNTypes.ISBN10)]
        public string ISBN10 { get; set; }

        [JsonProperty(PropertyName = "isbn13")]
        [ISBNValidation(ISBNType = ISBNTypes.ISBN13)]
        public string ISBN13 { get; set; }

        [StringLength(300, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 300 characters")]
        public string Description { get; set; }

        public string UserId { get; set; }

        public List<string> Authors { get; set; }

        [RangeAttribute(typeof(decimal), "0", "9223372036854775807", ErrorMessage = "Price must be above zero")]
        public decimal Price { get; set; }

        public string Condition { get; set; }

        public ContactOption ContactOption { get; set; }
    }
}