using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sXb_service.Models
{
    public class Listing
    {

        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        public Guid BookId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public string UserId { get; set; }

        [Range(0, 500.00, ErrorMessage = "Cannot be negative")]
        public decimal Price { get; set; }

        public Condition Condition { get; set; }

        [Required]
        public ContactOption ContactOption { get; set; } = ContactOption.SellerContactBuyer;
    }

    public enum Condition
    {
        New,
        [Display(Name = "Like New")]
        LikeNew,
        Good,
        Fair,
        Poor
    }

    public enum ContactOption
    {
        SellerContactBuyer,
        InternalMessaging
    }
}