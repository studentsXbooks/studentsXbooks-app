using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using sXb_service.Helpers.ModelValidation;

namespace sXb_service.Models.ViewModels
{
    public class CreateListingViewModel
    {
        public Guid Id { get; set; }

        [StringLength(265, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 256 characters")]
        public string Title { get; set; }

        [ISBNValidation(ISBNType = ISBNTypes.ISBN10)]
        public string ISBN10 { get; set; }

        public string ISBN13 { get; set; }

        [StringLength(300, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 300 characters")]
        public string Description { get; set; }

        public string UserId { get; set; }

        [StringLength(250, MinimumLength = 1, ErrorMessage = "Must be between 1 and 25 characters")]
        public string Authors { get; set; }

        [RangeAttribute(typeof(decimal), "0", "9223372036854775807", ErrorMessage = "Price must be above zero")]
        public decimal Price { get; set; }

        public Condition Condition { get; set; }
    }
}
