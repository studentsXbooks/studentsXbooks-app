using System;
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
using Moq;
using Newtonsoft.Json;
using sXb_service;
using sXb_service.Helpers;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.Services;
using Xunit;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Formatting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit.Abstractions;

namespace sXb_tests.Integration {
    public class UsersController : IClassFixture<WebApplicationFactory<Startup>> {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;
        private readonly ITestOutputHelper output;

        public UsersController (WebApplicationFactory<Startup> factory, ITestOutputHelper output) {
            _factory = factory;
            fixture = new Fixture ().Customize (new AutoMoqCustomization ());
            fixture.Customize<RegisterViewModel> ((ob) => ob.With (x => x.Email, $"{Guid.NewGuid()}@wvup.edu").With (x => x.Password, "Develop@90").With( x => x.Username, Guid.NewGuid().ToString()));
            this.output = output;
    }

        [Fact]
        public async Task Register_HappyPath_ShouldReturn201 () {
            string url = "/api/users/new";
            var client = _factory.CreateClient ();

            var newUser = fixture.Create<RegisterViewModel> ();
            var response = await client.PostAsJsonAsync<RegisterViewModel> (url, newUser);

            Assert.Equal (HttpStatusCode.Created, response.StatusCode);
            Assert.Equal ("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString ());
        }

        [Fact]
        public async Task Register_NonEduAddress_ShouldReturn400 () {
            string url = "/api/users/new";
            var client = _factory.CreateClient ();

            var newUser = fixture.Create<RegisterViewModel> ();
            newUser.Email = "email@hotmail.com";
            var response = await client.PostAsJsonAsync<RegisterViewModel>(url, newUser);

            var errorMessage = await response.Content.ReadAsAsync<ErrorMessage> ();
            Assert.Equal (HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull (errorMessage.Message);
        }

        [Fact]
        public async Task Register_UserNameTaken_ShouldReturn400 () {
            string url = "/api/users/new";
            var client = _factory.CreateClient ();

            var newUser = fixture.Create<RegisterViewModel> ();
            newUser.Username = "TestUser";
            var response = await client.PostAsJsonAsync<RegisterViewModel> (url, newUser);

            var errorMessage = await response.Content.ReadAsAsync<ErrorMessage> ();
            Assert.Equal (HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull (errorMessage.Message);
        }

        [Theory]
        [InlineData ("abc")]
        [InlineData ("1")]
        [InlineData ("")]
        [InlineData("abcde")]
        [InlineData("Abcde")]
        public async Task Register_PasswordTooShort_ShouldReturn400 (string password) {
            string url = "/api/users/new";
            var client = _factory.CreateClient ();

            var newUser = fixture.Create<RegisterViewModel> ();
            newUser.Password = password;
            var response = await client.PostAsJsonAsync<RegisterViewModel> (url, newUser);

            response.Content.Headers.ContentType.MediaType = "application/json";
            var errorMessage = await response.Content.ReadAsAsync<ValidationProblemDetails> ();
            Assert.Equal (HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull (errorMessage);
        }
        [Theory]
        [InlineData("Abcde1")]
        public async Task Register_PasswordWeak_ShouldReturn404(string password)
        {
            string url = "/api/users/new";
            var client = _factory.CreateClient();

            var newUser = fixture.Create<RegisterViewModel>();
            newUser.Password = password;
            var response = await client.PostAsJsonAsync<RegisterViewModel>(url, newUser);

            response.Content.Headers.ContentType.MediaType = "application/json";
            var errorMessage = await response.Content.ReadAsAsync<ValidationProblemDetails>();
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(errorMessage);
        }
        [Theory]
        [InlineData("AbcdeF1#")]
        [InlineData("Develop@90")]
        public async Task Register_PasswordStrong_ShouldReturn201(string password)
        {
            string url = "/api/users/new";
            var client = _factory.CreateClient();

            var newUser = fixture.Create<RegisterViewModel>();
            newUser.Password = password;
            var response = await client.PostAsJsonAsync<RegisterViewModel>(url, newUser);

            //response.Content.Headers.ContentType.MediaType = "application/json";
            var content = await response.Content.ReadAsAsync<Object>();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task Register_AlreadyRegisteredEmail_ShouldReturn400 () {
            string url = "/api/users/new";
            var client = _factory.CreateClient ();

            var newUser = fixture.Create<RegisterViewModel> ();

            newUser.Email = "test@wvup.edu";
            var response = await client.PostAsJsonAsync<RegisterViewModel> (url, newUser);

            var errorMessage = await response.Content.ReadAsAsync<ErrorMessage> ();
            Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
            Assert.NotNull (errorMessage.Message);
        }

        [Theory]
        [InlineData ( "", "", "")]
        [InlineData ("", "Develop@90", "")]
        [InlineData ("newUsername", "Develop@90", "")]
        [InlineData ("", "Develop@90", "newUser@wvup.edu")]
        [InlineData ("", "", "newUser@wvup.edu")]
        [InlineData ("newUsername", "", "")]
        public async Task Register_BadVM_ShouldReturn400 (string username, string password, string email) {
            string url = "/api/users/new";
            var client = _factory.CreateClient ();

            var newUser = new RegisterViewModel()
            {
                Email = email,
                Username = username,
                Password = password
            };

            HttpResponseMessage response = await client.PostAsJsonAsync<RegisterViewModel> (url, newUser);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            // Manually set the content-type since webapi fusses about reading an "application/problem+json."
            response.Content.Headers.ContentType.MediaType = "application/json";
            var errorMessage = await response.Content.ReadAsAsync<ValidationProblemDetails>();
            Assert.NotNull(errorMessage);
        }

        [Fact]
        public async Task EmailConfirm_InvalidToken_Return400WithNoCookie () {
            string url = "/api/users/new?userId=asfasdf&code=123123";
            var client = _factory.CreateClient ();

            HttpResponseMessage response = await client.GetAsync(url);
            Assert.Equal (HttpStatusCode.BadRequest, response.StatusCode);
            IEnumerable<string> cookies = null;
            response.Headers.TryGetValues("set-cookie", out cookies);
            Assert.Null(cookies);
        }

        [Fact]
        public async Task GetUserName_CallerHasCookie_Return200WithUserName () {
            string url = "/api/users/name";
            var client = _factory.CreateClient ();
            await client.PostAsJsonAsync<LoginViewModel> ("/api/users/", new LoginViewModel () {
                Email = "test@wvup.edu",
                    Password = "Develop@90"
            });

            var response = await client.GetAsync (url);
            var username = await response.Content.ReadAsAsync<User> ();
            Assert.Equal (HttpStatusCode.OK, response.StatusCode);
            Assert.Equal (username.UserName, "TestUser");
        }

        [Fact]
        public async Task GetUserName_CallerHasNoCookie_Return400 () {
            string url = "/api/users/name";
            var client = _factory.CreateClient ();

            var response = await client.GetAsync (url);
            Assert.Equal (HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetUserName_InvalidCookie_Return400 () {
            string url = "/api/users/name";
            var client = _factory.CreateClient ();
            var request = new HttpRequestMessage (HttpMethod.Get, url);
            request.Headers.Add ("Cookie", "AspnetIdentityCookie=Whutever;");

            var response = await client.SendAsync (request);
            Assert.Equal (HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task EmailConfirm_NewUserValidToken_ReturnRedirectAndUserAccountConfirmIsTrueAndCookieReturned()
        {
            // 1. Retrieve cookie & userId from registration process.
            // 2. Insert cookie & id into code query param.


            string url = "/api/users/new";
            var client = _factory.CreateClient();
            RegisterViewModel newUser = new RegisterViewModel()
            {
                Email = "newer@wvup.edu",
                Username = "newerUser",
                Password = "AbcdeF#1"
            };
            var response = await client.PostAsJsonAsync<RegisterViewModel>(url, newUser);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
;
            var data = await response.Content.ReadAsAsync<EmailConfirmViewModel>();
            output.WriteLine("ID : " + data.Id);
            output.WriteLine("Code : " + data.Code);
            //response.RequestMessage.Headers.GetCookies()[0].Cookies[0].Value;

            url = $"/api/users/new?userId={data.Id}&code={data.Code}";

            response = await client.GetAsync(url);
            // SXB-FRONT.COM:3000/Email-confirmed may be outside of xunit's reach.
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);


        }
    }
}