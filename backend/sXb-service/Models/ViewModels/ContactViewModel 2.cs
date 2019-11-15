using System;
using System.ComponentModel.DataAnnotations;

namespace sXb_service.ViewModels
{
    public class ContactViewModel2
    {
        [Required]
        [EmailAddress]
        // Email of person who is intersted in purchasing/trading Sellers book
        public string Email { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Body { get; set; }

        // '?' allows model validation to happen when no ListingId Is provided
        [Required]
        public Guid? ListingId { get; set; }
    }
}