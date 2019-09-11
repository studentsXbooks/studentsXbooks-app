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
        Guid guid = Guid.NewGuid();

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
        public async void CreateBook_HappyPath()
        {
            var book = fixture.Create<Book>();
            book.Id = guid;

            var res = await bookRepo.Add(book);

            Assert.Equal(book.Id, res.Id);
        }

        [Fact]
        public async void GetBook__HappyPath()
        {
            Guid bookId = Guid.Empty;
            var books = fixture.CreateMany<Book>();
            foreach (var book in books)
            {
                await bookRepo.Add(book);
                bookId = book.Id;
            }
            
            var myBook = await bookRepo.Find(bookId);

            Assert.Equal(bookId, myBook.Id);
        }

        [Fact]
        public async void GetAll__HappyPath()
        {
            var books = fixture.CreateMany<Book>();
            foreach(var book in books)
            {
               await bookRepo.Add(book);
            }           

            Assert.Equal(books, bookRepo.GetAll());
        }

        [Fact]
        public async void UpdateBook_HappyPath()
        {
            const string updateAuthor = "myNewAuthor";
            var book = fixture.Create<Book>();
            book.Id = guid;
            var createdBook = await bookRepo.Add(book);
            var originalAuthor = createdBook.Author;
            var updatedBook = createdBook;

            updatedBook.Author = updateAuthor;
            var updateRes = await bookRepo.Update(updatedBook);

            Assert.NotEqual(originalAuthor, updateRes.Author);
            Assert.Equal(updateRes.Author, updateAuthor);
        }

        [Fact]
        public async void BookExists__HappyPath()
        {
            Guid bookId = Guid.Empty;
            var books = fixture.CreateMany<Book>();
            foreach (var book in books)
            {
                await bookRepo.Add(book);
                bookId = book.Id;
            }

            Assert.True(await bookRepo.Exist(bookId));
        }

        [Fact]
        public async void RemoveBook__HappyPath()
        {
            Guid bookId = Guid.Empty;
            var books = fixture.CreateMany<Book>();
            foreach (var book in books)
            {
                await bookRepo.Add(book);
                bookId = book.Id;
            }

            await bookRepo.Remove(bookId);
            Assert.False(await bookRepo.Exist(bookId));
        }
    }
}
