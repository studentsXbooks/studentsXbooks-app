using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using sXb_service.Helpers;
using sXb_service.Models;

namespace sXb_service.ViewModels {
    public class ListingViewModel {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Range (0.0, 500.00, ErrorMessage = "Cannot be negative")]
        public decimal Price { get; set; }

        public Condition Condition { get; set; }

        public Guid BookId { get; set; }
    }
}