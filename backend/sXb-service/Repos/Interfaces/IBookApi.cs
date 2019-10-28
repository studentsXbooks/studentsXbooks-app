using sXb_service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sXb_service.Repos.Interfaces
{
    public interface IBookApi
    {
        Task<IEnumerable<BookApiResult>> FindBook(string term);
    }
}
