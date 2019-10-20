using Microsoft.EntityFrameworkCore;
using sXb_service.Models;
using sXb_service.Repos.Base;
using sXb_service.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Repos
{
    public class BookAuthorRepo : BaseRepo<BookAuthor>, IBookAuthorRepo
    {
        public BookAuthorRepo(DbContextOptions options) : base(options) { }

        protected override IQueryable<BookAuthor> Include(DbSet<BookAuthor> set)
        {
            throw new NotImplementedException();
        }
    }
}
