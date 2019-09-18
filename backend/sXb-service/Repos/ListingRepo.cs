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
    public class ListingRepo : BaseRepo<Listing>, IListingRepo

    {
        public ListingRepo(DbContextOptions options) : base(options) { }

        public async Task<Listing> Add(Listing listing)
        {
            await _db.AddAsync(listing);
            await _db.SaveChangesAsync();

            return listing;
        }

        public async override Task<bool> Exist(Guid id)
        {
            return await _db.Listings.AnyAsync(e => e.Id == id);
        }

        public async override Task<Listing> Find(Guid id)
        {
            return await _db.Listings.SingleOrDefaultAsync(e => e.Id == id);
        }

        public override IEnumerable<Listing> GetAll()
        {
            return _db.Listings;
        }

        public async override Task<Listing> Remove(Guid id)
        {
            var listing = await _db.Listings.SingleAsync(a => a.Id == id);
            _db.Listings.Remove(listing);
            await _db.SaveChangesAsync();
            return listing;
        }

        public async Task<Listing> Update(Listing listing)
        {
            _db.Listings.Update(listing);
            await _db.SaveChangesAsync();
            return listing;
        }

        //public async Task<IEnumerable<Listing>> ByUser(string userId)
        //{
        //    if (_db.Listings.Any(x => x.UserBook.UserId == userId))
        //    {
        //        return await _db.Listings.Where(e => e.UserBook.UserId == userId).Include(e => e.UserBook.Book).ToListAsync();
        //    }
        //    return null;
        //}

    }
}
