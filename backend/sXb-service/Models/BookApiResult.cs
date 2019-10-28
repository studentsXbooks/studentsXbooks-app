using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Models
{
    public class BookApiResult
    {
        public string Title { get; set; }

        public string ISBN10 { get; set; }

        public string ISBN13 { get; set; }

        public string Description { get; set; }

        public string SmallThumbnail { get; set; }

        public string Thumbnail { get; set; }

    }
}
