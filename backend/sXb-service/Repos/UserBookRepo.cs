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
    public class UserBookRepo : BaseRepo<UserBook>, IUserBookRepo
    {
        public UserBookRepo(DbContextOptions options) : base(options) { }

        public async Task<UserBook> Add(UserBook userBook)
        {
            await _db.AddAsync(userBook);
            await _db.SaveChangesAsync();

            return userBook;
        }

        public async override Task<bool> Exist(Guid id)
        {
            return await _db.UserBooks.AnyAsync(e => e.Id == id);
        }

        public async override Task<UserBook> Find(Guid id)
        {
            return await _db.UserBooks.SingleOrDefaultAsync(e => e.Id == id);
        }

        public override IEnumerable<UserBook> GetAll()
        {
            return _db.UserBooks;
        }

        public async override Task<UserBook> Remove(Guid id)
        {
            var userBook = await _db.UserBooks.SingleAsync(a => a.Id == id);
            _db.UserBooks.Remove(userBook);
            await _db.SaveChangesAsync();
            return userBook;
        }

        public async Task<UserBook> Update(UserBook userBook)
        {
            _db.UserBooks.Update(userBook);
            await _db.SaveChangesAsync();
            return userBook;
        }
    }
}
