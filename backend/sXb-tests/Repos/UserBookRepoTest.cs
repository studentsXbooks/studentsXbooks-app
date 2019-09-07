using AutoFixture;
using AutoFixture.AutoMoq;
using sXb_service.EF;
using sXb_service.Models;
using sXb_service.Repos;
using sXb_tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace sXb_tests.Repos
{
    public class UserBookRepoTest : IDisposable
    {
        UserBookRepo userBookRepo;

        string dbName = "UserBookTest";

        IFixture fixture;
        Guid guid = Guid.NewGuid();

        TxtXContext db;

        public UserBookRepoTest()
        {
            var dbOptions = DbInMemory.getDbInMemoryOptions(dbName);
            db = new TxtXContext(dbOptions);
            userBookRepo = new UserBookRepo(DbInMemory.getDbInMemoryOptions(dbName));
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        public void Dispose()
        {
            db.Database.EnsureDeleted();
        }

        [Fact]
        public async void CreateUserBook_HappyPath()
        {
            var userBook = fixture.Create<UserBook>();
            userBook.Id = guid;

            var res = await userBookRepo.Add(userBook);

            Assert.Equal(userBook.Id, res.Id);
        }

        [Fact]
        public async void GetUserBook__HappyPath()
        {
            Guid userBookId = Guid.Empty;
            var userBooks = fixture.CreateMany<UserBook>();
            foreach (var userBook in userBooks)
            {
                await userBookRepo.Add(userBook);
                userBookId = userBook.Id;
            }

            var myUserBook = await userBookRepo.Find(userBookId);

            Assert.Equal(userBookId, myUserBook.Id);
        }

        [Fact]
        public async void GetAll__HappyPath()
        {
            var userBooks = fixture.CreateMany<UserBook>();
            foreach (var userBook in userBooks)
            {
                await userBookRepo.Add(userBook);
            }

            Assert.Equal(userBooks, userBookRepo.GetAll());
        }

        [Fact]
        public async void UpdateUserBook_HappyPath()
        {
            Guid updateBookId = Guid.NewGuid();
            var userBook = fixture.Create<UserBook>();
            userBook.Id = guid;
            var createdUserBook = await userBookRepo.Add(userBook);
            var originalBookId = createdUserBook.BookId;
            var updatedUserBook = createdUserBook;

            updatedUserBook.BookId = updateBookId;
            var updateRes = await userBookRepo.Update(updatedUserBook);

            Assert.NotEqual(originalBookId, updateRes.BookId);
            Assert.Equal(updateRes.BookId, updateBookId);
        }

        [Fact]
        public async void UserBookExists__HappyPath()
        {
            Guid userBookId = Guid.Empty;
            var userBooks = fixture.CreateMany<UserBook>();
            foreach (var userBook in userBooks)
            {
                await userBookRepo.Add(userBook);
                userBookId = userBook.Id;
            }

            Assert.True(await userBookRepo.Exist(userBookId));
        }

        [Fact]
        public async void RemoveUserBook__HappyPath()
        {
            Guid userBookId = Guid.Empty;
            var userBooks = fixture.CreateMany<UserBook>();
            foreach (var userBook in userBooks)
            {
                await userBookRepo.Add(userBook);
                userBookId = userBook.Id;
            }

            await userBookRepo.Remove(userBookId);
            Assert.False(await userBookRepo.Exist(userBookId));
        }
    }
}
