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
    [Collection("Integration")]
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
            string listingsByUserUrl = "/api/listings/user/1";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });

            var response = await client.GetAsync(listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(listings);
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookieAndPageTwo_Return200WithAPageOfListingVMs()
        {
            string listingsByUserUrl = "/api/listings/user/2";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });

            var response = await client.GetAsync(listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(listings);
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookieAndNoPageData_Return200WithAPageOfNoData()
        {
            string listingsByUserUrl = "/api/listings/user/1";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "nolistingsuser@wvup.edu",
                Password = "Develop@90"
            });

            var response = await client.GetAsync(listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(listings.Data);
        }

        [Fact]
        public async Task GetListingDetail_IdExists_Return200WithListingDetailVM()
        {
            var listing = fixture.Create<ListingViewModel>();
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/listings/a059efc3-a4ec-4abf-946a-84194b2e0a00");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetListingDetail_IdNotFound_Return404()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/listings/81c92cf8-d6c2-4b10-ad00-3eaac26d9b92");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ValidListingDetail_Returns201WithModel()
        {
            string url = "/api/listings/";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });
            var newListing = fixture.Create<CreateListingViewModel>();
            newListing.Description = "Short book description";
            newListing.FirstName = "Author";
            newListing.MiddleName = "B";
            newListing.LastName = "BookWriter";
            newListing.ISBN10 = "1082148938";
            newListing.Price = 5.99m;
            newListing.Title = "Short Title";
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
        [InlineData("Normal Length Title", "FirstName", "LastName", "MiddleName", "978746062760", 4.99, Condition.Fair)] // Should fail cause ISBN10 not valid
        [InlineData("Normal Length Title", "FirstName", "LastName", "MiddleName", "978746062760", -4.99, Condition.Fair)]
        public async Task Create_InvalidListingDetail_Return400(string title, string firstName, string lastName, string middleName, string isbn, decimal price,
            Condition condition)
        {
            string url = "/api/listings/";
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });

            var newListing = new CreateListingViewModel()
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Title = title,
                ISBN10 = isbn,
                Price = price,
                Condition = condition
            };
            var response = await client.PostAsJsonAsync<CreateListingViewModel>(url, newListing);

            var errorMessage = await response.Content.ReadAsAsync<ValidationResultModel>();
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.NotNull(errorMessage);
        }
    }
}