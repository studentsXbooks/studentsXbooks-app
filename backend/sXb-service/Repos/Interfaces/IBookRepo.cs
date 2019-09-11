using sXb_service.Models;
using sXb_service.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Repos.Interfaces
{
    public interface IBookRepo : IRepo<Book>
    {
        Task<Book> Add(Book book);

        Task<Book> Update(Book book);
    }
}
