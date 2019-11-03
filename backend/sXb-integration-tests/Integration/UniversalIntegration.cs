using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.Testing;
using sXb_service;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace sXb_integration_tests.Integration
{
    [Collection("Integration")]
    public class UniversalIntegration : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;

        public UniversalIntegration(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Theory]
        [InlineData("api/listings/")]
        public async Task PostUser_NotLoggedIn_Return401(string api)
        {
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync(api, "myData");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/listings/user/1")]
        public async Task GetUser_NotLoggedIn_Return401(string api)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(api);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
