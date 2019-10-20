﻿using Microsoft.EntityFrameworkCore;
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
    public class BookRepo : BaseRepo<Book>, IBookRepo
    {
        public BookRepo(DbContextOptions options) : base(options) { }

        protected override IQueryable<Book> Include(DbSet<Book> set) => set.Include(x => x.BookAuthors).ThenInclude(x => x.Author);

    }
}
