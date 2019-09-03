using System;
using sXb_service.Repos;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Xunit;
using sXb_service.EF;
using sXb_tests.Helpers;
using AutoFixture.AutoMoq;
using sXb_service.Models;

namespace sXb_tests.Repos
{
    public class BookRepoTest : IDisposable
    {
        BookRepo bookRepo;

        string dbName = "BookRepoTest";

        IFixture fixture;

        TxtXContext db;

        public BookRepoTest()
        {
           var dbOptions = DbInMemory.getDbInMemoryOptions(dbName);
            db = new TxtXContext(dbOptions);
            bookRepo = new BookRepo(DbInMemory.getDbInMemoryOptions(dbName));
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        public void Dispose()
        {
            db.Database.EnsureDeleted();
        }

        [Fact]
        public async void CreateBook_ShouldWork()
        {
            var book = fixture.Create<Book>();

            var res = await bookRepo.Add(book);

            Assert.Equal(book.ISBN, res.ISBN);
        }
    }
}
