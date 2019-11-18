using sXb_service.Helpers;
using sXb_service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sXb_service.Repos.Interfaces
{
    public interface IBookApi
    {
        Task<Paging<BookApiResult>> FindBook(string term, int page);
    }
}
