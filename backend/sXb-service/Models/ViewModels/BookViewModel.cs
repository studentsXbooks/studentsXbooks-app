using System;

namespace sXb_service.ViewModels
{
    public class BookViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public string ImageURL { get; set; }
    }
}
