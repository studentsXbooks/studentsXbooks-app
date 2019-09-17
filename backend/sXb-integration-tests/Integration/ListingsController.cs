using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using sXb_service;
using sXb_service.Helpers;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.ViewModels;
using Xunit;

namespace sXb_tests.Integration
{
    public class ListingsController : IClassFixture<WebApplicationFactory<Startup>> {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;

        public ListingsController (WebApplicationFactory<Startup> factory) {
            _factory = factory;
            fixture = new Fixture ().Customize (new AutoMoqCustomization ());
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
    }
}