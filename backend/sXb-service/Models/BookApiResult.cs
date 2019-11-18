using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace sXb_service.Models
{
    public class BookApiResult
    {
        public string Title { get; set; }

        [JsonProperty(PropertyName = "isbn10")]
        public string ISBN10 { get; set; }

        [JsonProperty(PropertyName = "isbn13")]
        public string ISBN13 { get; set; }

        public string Description { get; set; }

        public string SmallThumbnail { get; set; }

        public string Thumbnail { get; set; }
        public string[] Authors { get; set; }

    }
}