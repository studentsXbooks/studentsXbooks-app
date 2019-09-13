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
using sXb_service.Helpers;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using Xunit;

namespace sXb_tests.Integration {
    public class UsersController : IClassFixture<WebApplicationFactory<Startup>> {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;

        public UsersController (WebApplicationFactory<Startup> factory) {
            _factory = factory;
            fixture = new Fixture ().Customize (new AutoMoqCustomization ());
        }

        [Fact]
        public async Task Register_HappyPath_ShouldReturn201 () {
            string url = "api/users/new";
            var client = _factory.CreateClient ();

            var newUser = fixture.Create<RegisterViewModel> ();
            var response = await client.PostAsJsonAsync<RegisterViewModel> (url, newUser);

            Assert.Equal (HttpStatusCode.Created, response.StatusCode);
            Assert.Equal ("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString ());
        }

        [Fact]
        public async Task Register_NonEduAddress_ShouldReturn400 () {
            string url = "api/users/new";
            var client = _factory.CreateClient ();

            var newUser = fixture.Create<RegisterViewModel> ();
            newUser.Email = "email@hotmail.com";
            var response = await client.PostAsJsonAsync<RegisterViewModel> (url, newUser);
            var errorMessage = await response.Content.ReadAsAsync<ErrorMessage> ();
            Assert.Equal (HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull (errorMessage.Message);
        }

        [Fact]
        public async Task Register_UserNameTaken_ShouldReturn400 () {

        }

        [Theory]
        [InlineData ("as")]
        public async Task Register_PasswordWeak_ShouldReturn400 (string password) {

        }

        [Fact]
        public async Task Register_AccountAlreadyExists_ShouldReturn400 () {

        }

        [Theory]
        [InlineData ("", "", "")]
        public async Task Register_BadVM_ShouldReturn400 (string username, string password, string email) {

        }

        [Fact]
        public async Task Register_EmailConfirmThrowsError_ShouldReturn400 () {

        }

        [Fact]
        public async Task EmailConfirm_NewUserValidToken_Return200AndUserAccountConfirmIsTrueAndCookieReturned () {

        }

        [Fact]
        public async Task EmailConfirm_InvalidToken_Return400 () {

        }

        [Fact]
        public async Task GetUserName_CallerHasCookie_Return200WithUserName () {

        }

        [Fact]
        public async Task GetUserName_CallerHasNoCookie_Return400 () {

        }

        [Fact]
        public async Task GetUserName_InvalidCookie_Return400 () {

        }

    }
}