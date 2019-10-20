using System;
using System.ComponentModel.DataAnnotations;
using sXb_service.Models;

namespace sXb_service.ViewModels {
    public class ListingViewModel {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Range (0.0, 500.00, ErrorMessage = "Cannot be negative")]
        public decimal Price { get; set; }

        public Condition Condition { get; set; }

        public Guid BookId { get; set; }

        public Status Status { get; set; }
    }
}