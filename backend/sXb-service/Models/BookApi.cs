using AutoMapper;
using sXb_service.Helpers;
using sXb_service.Repos.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace sXb_service.Models
{
    public class BookApi : IBookApi
    {
        private string key;
        private HttpClient bookApi;
        private IMapper _mapper;

        public BookApi(IHttpClientFactory clientFactory, BookApiConfig bookApiConfig, IMapper mapper)
        {
            bookApi = clientFactory.CreateClient("findBook");
            key = bookApiConfig.Apikey;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookApiResult>> FindBook(string term)
        {
            var response = await bookApi.GetAsync($"volumes?q={term}&key={key}");
            response.EnsureSuccessStatusCode();
            var bookInfo = await response.Content.ReadAsAsync<BookSearchResults>();

            List<BookApiResult> results = new List<BookApiResult>();

            foreach(var book in bookInfo.Items)
            {
                var newBook = _mapper.Map<BookApiResult>(book.VolumeInfo);
                results.Add(newBook);
            }

            return results;
        }
    }
}

public class BookSearchResults
{
    public Items[] Items { get; set; }
}


public class Items
{
    public VolumeInfo VolumeInfo { get; set; }
}

public class VolumeInfo
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string[] Authors { get; set; }
    public string Publisher { get; set; }
    public string Description { get; set; }
    public IndustryIdentifiers[] IndustryIdentifiers { get; set; }
    public ImageLinks ImageLinks { get; set; }

}

public class IndustryIdentifiers
{
    public string Type { get; set; }
    public string Identifier { get; set; }
}

public class ImageLinks
{
    public string SmallThumbnail { get; set; }
    public string Thumbnail { get; set; }
    
}
