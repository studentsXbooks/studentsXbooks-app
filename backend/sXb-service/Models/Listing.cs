using sXb_service.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models {
    public class Listing {

        [Key]
        public Guid Id { get; set; }

        [ForeignKey (nameof (BookId))]
        public Book Book { get; set; }

        public Guid BookId { get; set; }

        [ForeignKey (nameof (UserId))]
        public User User { get; set; }

        public string UserId { get; set; }

        [Range (0, 500.00, ErrorMessage = "Cannot be negative")]
        public decimal Price { get; set; }

        public Condition Condition { get; set; }
    }

    public enum Condition {
        New,
        LikeNew,
        Good,
        Fair,
        Poor
    }
}