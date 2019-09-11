using System.Collections.Generic;
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
using sXb_service.Models;
using sXb_service.ViewModels;
using Xunit;

namespace sXb_tests.Integration {
    public class ListingsController : IClassFixture<WebApplicationFactory<Startup>> {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;

        public ListingsController (WebApplicationFactory<Startup> factory) {
            _factory = factory;
            fixture = new Fixture ().Customize (new AutoMoqCustomization ());
        }

        [Theory]
        [InlineData ("/api/listings")]
        [InlineData ("/api/listings/b55146c3-dfef-4854-b2c6-a657fdd44e5d")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType (string url) {
            // Arrange
            var client = _factory.CreateClient ();

            // Act
            var response = await client.GetAsync (url);

            // Assert
            response.EnsureSuccessStatusCode (); // Status Code 200-299
            Assert.Equal ("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString ());
        }

        [Fact]
        public async Task Post_EndpointReturnCreatedAndCorrectContentType () {
            var listing = fixture.Create<ListingViewModel> ();
            var client = _factory.CreateClient ();
            var listingJson = JsonConvert.SerializeObject (listing);

            var body = new StringContent (listingJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync ("/api/listings", body);

            response.EnsureSuccessStatusCode ();
            Assert.Equal (HttpStatusCode.Created, response.StatusCode);
        }
    }
}