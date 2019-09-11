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
    public class BookRepo : BaseRepo<Book>, IBookRepo
    {
        public BookRepo(DbContextOptions options) : base(options) { }
        
        public async Task<Book> Add(Book book)
        {
            await _db.AddAsync(book);
            await _db.SaveChangesAsync();

            return book;
        }

        public async override Task<bool> Exist(Guid id)
        {
            return await _db.Books.AnyAsync(e => e.Id == id);
        }

        public async override Task<Book> Find(Guid id)
        {
            return await _db.Books.SingleOrDefaultAsync(e => e.Id == id);
        }

        public override IEnumerable<Book> GetAll()
        {
            return _db.Books;
        }

        public async override Task<Book> Remove(Guid id)
        {
            var book = await _db.Books.SingleAsync(a => a.Id == id);
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<Book> Update(Book book)
        {
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
            return book;
        }
    }
}
