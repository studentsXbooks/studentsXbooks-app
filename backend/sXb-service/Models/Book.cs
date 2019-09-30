using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }
            
        public string ISBN10 { get; set; }

        public string Description { get; set; }

        [InverseProperty(nameof(BookAuthor.Book))]
        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    }
}
