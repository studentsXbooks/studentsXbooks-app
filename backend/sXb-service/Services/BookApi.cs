using AutoMapper;
using sXb_service.Helpers;
using sXb_service.Models;
using sXb_service.Repos.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace sXb_service.Services
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

        public async Task<Paging<BookApiResult>> FindBook(string term, int page)
        {
            List<BookApiResult> results = new List<BookApiResult>();
            var index = (page - 1) * 10;
            try
            {
                var response = await bookApi.GetAsync($"volumes?q={term}&startIndex={index}&key={key}");
                response.EnsureSuccessStatusCode();
                var bookInfo = await response.Content.ReadAsAsync<BookSearchResults>();
                foreach (var book in bookInfo.Items)
                {
                    var newBook = _mapper.Map<BookApiResult>(book.VolumeInfo);
                    if(newBook.ISBN10 != null)
                    {
                        results.Add(newBook);
                    }
                }
                return Paging<BookApiResult>.ApplyPaging(bookInfo.TotalItems, results, page);
            }
            catch
            {
                return Paging<BookApiResult>.ApplyPaging(0, results, page);
            }
        }          
    }
}

public class BookSearchResults
{
    public int TotalItems { get; set; }
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
