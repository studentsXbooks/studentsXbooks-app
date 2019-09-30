using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Testing;
using sXb_service;
using sXb_service.Helpers;
using sXb_service.Helpers.ModelValidation;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.ViewModels;
using Xunit;

namespace sXb_tests.Integration
{
    public class ListingsController : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;

        public ListingsController(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookie_Return200WithAPageOfListingVMs()
        {
            string listingsByUserUrl = "/api/listings/user";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });

            var response = await client.GetAsync(listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<Listing>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, listings.Data.Count());
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookieAndNoPageData_Return200WithAPageOfNoData()
        {
            string listingsByUserUrl = "/api/listings/user";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "nolistingsuser@wvup.edu",
                Password = "Develop@90"
            });

            var response = await client.GetAsync(listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<Listing>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(listings.Data);
        }

        [Fact]
        public async Task GetListingDetail_IdExists_Return200WithListingDetailVM()
        {
            var listing = fixture.Create<ListingViewModel>();
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/listings/b55146c3-dfef-4854-b2c6-a657fdd44e5d");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetListingDetail_IdNotFound_Return400()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/listings/1");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_ValidListingDetail_Returns201WithModel()
        {
            string url = "/api/listings/";
            var client = _factory.CreateClient();
            var newListing = fixture.Create<ListingDetailsViewModel>();
            newListing.UserId = "1234";
            var response = await client.PostAsJsonAsync(url, newListing);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("This title is way too long and the test should fail because of it because it is really long and it can only be 256 characters but it is longer than that and so it should return a bad request instead of working because this won't work because it is too long and this is a good test so it should not work", "FirstName", "LastName", "middleName", "9780746062760", 4.99, Condition.Fair)]
        [InlineData("", "FirstName", "LastName", "middleName", "9780746062760", 4.99, Condition.Good)]
        [InlineData("Normal Length Title", "", "LastName", "middleName", "9780746062760", 4.99, Condition.Fair)]
        [InlineData("Normal Length Title", "FirstName", "", "middleName", "9780746062760", 4.99, Condition.Fair)]
        [InlineData("Normal Length Title", "FirstName", "LastName", "", "9780746062760", 4.99, Condition.Fair)]
        [InlineData("Normal Length Title", "FirstName", "LastName", "MiddleName", "978746062760", 4.99, Condition.Fair)]
        [InlineData("Normal Length Title", "FirstName", "LastName", "MiddleName", "978746062760", -4.99, Condition.Fair)]
        public async Task Create_InvalidListingDetail_Return400(string title, string firstName, string lastName, string middleName, string isbn, decimal price,
            Condition condition)
        {
            string url = "/api/listings/";
            var client = _factory.CreateClient();

            var newListing = new ListingDetailsViewModel()
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Title = title,
                ISBN10 = isbn,
                Price = price,
                Condition = condition
            };
            var response = await client.PostAsJsonAsync<ListingDetailsViewModel>(url, newListing);

            var errorMessage = await response.Content.ReadAsAsync<ValidationResultModel>();
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.NotNull(errorMessage);
        }
    }
}