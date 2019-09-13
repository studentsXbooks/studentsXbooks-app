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

        [Fact]
        public async Task GetUsersListing_CallerHasCookie_Return200WithAPageOfListingVMs () {

        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookieAndNoPageData_Return200WithAPageOfNoData () {

        }

        [Fact]
        public async Task GetUsersListing_CallerHasNoCookie_Return400 () {

        }

        [Fact]
        public async Task GetUsersListing_InvalidCookie_Return400 () {

        }

        [Fact]
        public async Task GetListingDetail_IdExists_Return200WithListingDetailVM () {

        }

        [Fact]
        public async Task GetListingDetail_IdNotFound_Return400 () {

        }
    }
}