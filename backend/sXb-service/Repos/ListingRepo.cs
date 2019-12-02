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
    public class ListingRepo : BaseRepo<Listing>, IListingRepo
    {
        public ListingRepo(DbContextOptions options) : base(options) { }

        protected override IQueryable<Listing> Include(DbSet<Listing> set) => set.Include(x => x.Book).Include(x => x.User);
    }
}
