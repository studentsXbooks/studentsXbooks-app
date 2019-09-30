using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sXb_service.Models;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models
{
    public class BookAuthor
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        public Guid BookId { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required]
        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; }
    }
}
