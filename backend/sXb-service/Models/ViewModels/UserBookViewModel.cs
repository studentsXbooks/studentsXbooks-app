using sXb_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.ViewModels
{
    public class UserBookViewModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public Guid BookId { get; set; }

        public Condition Condition { get; set; }
    }
}
