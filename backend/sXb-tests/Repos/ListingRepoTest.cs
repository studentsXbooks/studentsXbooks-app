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
    public class ListingRepoTest : IDisposable
    {
        ListingRepo listingRepo;

        string dbName = "ListingRepoTest";

        IFixture fixture;
        Guid guid = Guid.NewGuid();

        TxtXContext db;

        public ListingRepoTest()
        {
            var dbOptions = DbInMemory.getDbInMemoryOptions(dbName);
            db = new TxtXContext(dbOptions);
            listingRepo = new ListingRepo(DbInMemory.getDbInMemoryOptions(dbName));
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        public void Dispose()
        {
            db.Database.EnsureDeleted();
        }

        [Fact]
        public async void CreateListing_HappyPath()
        {
            var listing = fixture.Create<Listing>();
            listing.Id = guid;

            var res = await listingRepo.Add(listing);

            Assert.Equal(listing.Id, res.Id);
        }

        [Fact]
        public async void GetListing__HappyPath()
        {
            Guid listingId = Guid.Empty;
            var listings = fixture.CreateMany<Listing>();
            foreach (var listing in listings)
            {
                await listingRepo.Add(listing);
                listingId = listing.Id;
            }

            var myListing = await listingRepo.Find(listingId);

            Assert.Equal(listingId, myListing.Id);
        }

        [Fact]
        public async void GetAll__HappyPath()
        {
            var listings = fixture.CreateMany<Listing>();
            foreach (var listing in listings)
            {
                await listingRepo.Add(listing);
            }

            Assert.Equal(listings, listingRepo.GetAll());
        }

        [Fact]
        public async void UpdateListing_HappyPath()
        {
            const decimal updatePrice = (decimal) 55.00;
            var listing = fixture.Create<Listing>();
            listing.Id = guid;
            var createdListing = await listingRepo.Add(listing);
            var originalPrice = createdListing.Price;
            var updatedListing = createdListing;

            updatedListing.Price = updatePrice;
            var updateRes = await listingRepo.Update(updatedListing);

            Assert.NotEqual(originalPrice, updateRes.Price);
            Assert.Equal(updateRes.Price, updatePrice);
        }

        [Fact]
        public async void ListingExists__HappyPath()
        {
            Guid listingId = Guid.Empty;
            var listings = fixture.CreateMany<Listing>();
            foreach (var listing in listings)
            {
                await listingRepo.Add(listing);
                listingId = listing.Id;
            }

            Assert.True(await listingRepo.Exist(listingId));
        }

        [Fact]
        public async void RemoveListing__HappyPath()
        {
            Guid listingId = Guid.Empty;
            var listings = fixture.CreateMany<Listing>();
            foreach (var listing in listings)
            {
                await listingRepo.Add(listing);
                listingId = listing.Id;
            }

            await listingRepo.Remove(listingId);
            Assert.False(await listingRepo.Exist(listingId));
        }

        [Fact]
        public async void GetByUser__HappyPath()
        {
            var userBook = fixture.Create<UserBook>();
            userBook.UserId = Guid.NewGuid().ToString();
            var listings = fixture.CreateMany<Listing>();
            foreach (var listing in listings)
            {
                listing.UserBook = userBook;
                await listingRepo.Add(listing);
            }
            var myListings = await listingRepo.ByUser(userBook.UserId);
            Assert.Equal(listings, myListings);
        }
    }
}
