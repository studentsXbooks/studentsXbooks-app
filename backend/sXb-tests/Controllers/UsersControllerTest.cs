using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using sXb_service.Controllers;
using sXb_service.EF;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.Repos.Interfaces;
using sXb_service.Services;
using sXb_tests.Helpers;
using Xunit;

namespace sXb_tests.Controllers {
    public class UsersControllerTest {
        IFixture fixture;
        UsersController usersController;

        IUserRepo userRepo;
        UserManager<User> userManager;
        SignInManager<User> signInManager;

        public UsersControllerTest () {
            fixture = new Fixture ().Customize (new AutoMoqCustomization ());
            userRepo = new Mock<IUserRepo> ().Object;
            //userManager = new Mock<UserManager<User>> ().Object;

            var userManagerMock = new Mock<UserManager<User>> (
                new Mock<IUserStore<User>> ().Object,
                new Mock<IOptions<IdentityOptions>> ().Object,
                new Mock<IPasswordHasher<User>> ().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer> ().Object,
                new Mock<IdentityErrorDescriber> ().Object,
                new Mock<IServiceProvider> ().Object,
                new Mock<ILogger<UserManager<User>>> ().Object);

            userManagerMock.Setup ((mock) => mock.CreateAsync (It.IsAny<User> (), It.IsAny<String> ())).ReturnsAsync ((User user, String password) => {
                user.Id = "5";
                return IdentityResult.Success;
            });
            userManagerMock.Setup ((mock) => mock.GenerateEmailConfirmationTokenAsync (It.IsAny<User> ()))
                .ReturnsAsync ("asdfasdf");
            userManager = userManagerMock.Object;

            signInManager = new Mock<SignInManager<User>> (userManager,
                new Mock<IHttpContextAccessor> ().Object,
                new Mock<IUserClaimsPrincipalFactory<User>> ().Object,
                new Mock<IOptions<IdentityOptions>> ().Object,
                new Mock<ILogger<SignInManager<User>>> ().Object,
                new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider> ().Object).Object;
        }

        [Fact]
        public async Task Register_EmailConfirmAsync_CalledOnce () {
            var emailSender = new Mock<IEmailSender> ();
            emailSender.Setup (mock => mock.SendEmailAsync (It.IsAny<string> (), It.IsAny<string> (), It.IsAny<string> ())).Returns (Task.CompletedTask);
            usersController = new UsersController (userRepo, userManager, signInManager, emailSender.Object);
            var newUser = fixture.Create<RegisterViewModel> ();

            await usersController.Register (newUser);

            emailSender.Verify ((mock) => mock.SendEmailAsync (It.IsAny<string> (), It.IsAny<string> (), It.IsAny<string> ()), Times.Once);
        }

        [Fact]
        public async Task Register_SendEmailThrowsError_Returns400 () {
            var emailSender = new Mock<IEmailSender> ();
            emailSender.Setup (mock => mock.SendEmailAsync (It.IsAny<string> (), It.IsAny<string> (), It.IsAny<string> ())).ThrowsAsync (new Exception ());
            usersController = new UsersController (userRepo, userManager, signInManager, emailSender.Object);
            var newUser = fixture.Create<RegisterViewModel> ();

            var result = await usersController.Register (newUser);
            Assert.IsType<BadRequestObjectResult> (result);
        }

        [Fact]
        public async Task EmailConfirm_NewUserValidToken_ReturnRedirectAndUserAccountConfirmIsTrueAndCookieReturned () {
            // 1. Retrieve cookie & userId from registration process.
            // 2. Insert cookie & id into code query param. 

            // string cookie = "";
            // string id = "";
            // string url = $"/api/users/new?userId={id}&code={cookie}";
            // var client = _factory.CreateClient ();
            // var response = await client.GetAsync (url);
            // Assert.Equal (HttpStatusCode.Redirect, response.StatusCode);
            // return null;
        }
    }
}