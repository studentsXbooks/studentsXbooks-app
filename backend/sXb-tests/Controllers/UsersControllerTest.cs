using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using sXb_service.EF;
using sXb_service.Models;
using sXb_service.Repos;
using sXb_tests.Helpers;
using Xunit;

namespace sXb_tests.Controllers {
    public class UsersControllerTest {
        IFixture fixture;

        public UsersControllerTest () {
            fixture = new Fixture ().Customize (new AutoMoqCustomization ());
        }

        [Fact]
        public void EmailConfirm_EmailConfirmAsync_CalledOnce () {
            //Given

            //When

            //Then
        }

        [Fact]
        public void Register_SendEmailThrowsError_Returns400 () {

        }

        [Fact]
        public async Task EmailConfirm_NewUserValidToken_ReturnRedirectAndUserAccountConfirmIsTrueAndCookieReturned () {
            // 1. Retrieve cookie & userId from registration process.
            // 2. Insert cookie & id into code query param. 

            string cookie = "";
            string id = "";
            string url = $"/api/users/new?userId={id}&code={cookie}";
            var client = _factory.CreateClient ();
            var response = await client.GetAsync (url);
            Assert.Equal (HttpStatusCode.Redirect, response.StatusCode);
        }
    }
}