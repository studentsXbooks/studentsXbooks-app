using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models
{
    public class Author
    {
        [Key]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string MiddleName { get; set; }

        public string FullName { get => FirstName + (String.IsNullOrEmpty(MiddleName) ? " " :   " " + MiddleName[0].ToString() + " " )+ LastName; }

        [InverseProperty(nameof(BookAuthor.Author))]
        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
