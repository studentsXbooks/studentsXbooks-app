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

        [StringLength(300, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 300 characters")]
        public string Description { get; set; }

        public string UserId { get; set; }

        [StringLength(25, MinimumLength = 1, ErrorMessage = "First Name must be between 1 and 25 characters")]
        public string FirstName { get; set; }

        [StringLength(25, MinimumLength = 1, ErrorMessage = "Last Name must be between 1 and 25 characters")]
        public string LastName { get; set; }

        [StringLength(25, MinimumLength = 1, ErrorMessage = "Middle Name must be between 1 and 25 characters")]
        public string MiddleName { get; set; }

        [RangeAttribute(typeof(decimal), "0", "9223372036854775807", ErrorMessage = "Price must be above zero")]
        public decimal Price { get; set; }

        public Condition Condition { get; set; }
    }
}
