using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.Testing;
using sXb_service;
using sXb_service.Helpers;
using sXb_service.Models;
using Xunit;

namespace sXb_integration_tests.Integration
{
    [Collection("Integration")]
    public class ConditionsControllerIntegration : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;

        public ConditionsControllerIntegration(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async void GetAll_ShouldReturn200WithListOfConditionEnum()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/conditions");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var conditionsFromApi = await response.Content.ReadAsAsync<IEnumerable<EnumNameValue>>();
            var conditionEnums = EnumExtensions.GetValues<Condition>();

            Assert.NotEmpty(conditionsFromApi);
            Assert.Equal(conditionEnums.Select(x => x.ToString()), conditionsFromApi.Select(x => x.Name));
            Assert.Equal(conditionEnums.Cast<int>(), conditionsFromApi.Select(x => x.Value).ToArray());
        }
    }
}