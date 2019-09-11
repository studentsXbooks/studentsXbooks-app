using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models
{
    public class UserBook
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        public Guid BookId { get; set; }

        public Condition Condition { get; set; }
    }

    public enum Condition
    {
        New,
        LikeNew,
        Good,
        Fair,
        Poor
    }
}
