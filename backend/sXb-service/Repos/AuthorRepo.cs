using Microsoft.EntityFrameworkCore;
using sXb_service.Models;
using sXb_service.Repos.Base;
using sXb_service.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace sXb_service.Repos
{
    public class AuthorRepo : BaseRepo<Author>, IAuthorRepo
    {
        public AuthorRepo(DbContextOptions options) : base(options) { }

        protected override IQueryable<Author> Include(DbSet<Author> set)
        {
            throw new NotImplementedException();
        }
    }
}
