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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using sXb_service.Controllers;
using sXb_service.EF;
using sXb_service.Helpers;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.Repos.Interfaces;
using sXb_service.Services;
using sXb_tests.Helpers;
using Xunit;

namespace sXb_tests.Controllers
{
    public class UsersControllerTest
    {
        IFixture fixture;
        UsersController usersController;

        IUserRepo userRepo;
        UserManager<User> userManager;
        SignInManager<User> signInManager;
        IConfiguration Configuration;

        public UsersControllerTest()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            userRepo = new Mock<IUserRepo>().Object;
            var mockConfiguration = new Mock<IConfiguration>();
            var fakeValues = new Mock<IConfigurationSection>();
            // Mocking IConfiguration is a pain
            // Somehow this works even though FrontendDomain isn't set when parse by Configuration.Get("")
            fakeValues.Setup(a => a.Key).Returns("FrontendDomain");
            fakeValues.Setup(a => a.Value).Returns("something");
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.GetChildren()).Returns(new List<IConfigurationSection>() { fakeValues.Object });
            mockConfiguration.Setup(a => a.GetSection("Cors")).Returns(configurationSection.Object);
            Configuration = mockConfiguration.Object;

            var userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);

            userManagerMock.Setup((mock) => mock.CreateAsync(It.IsAny<User>(), It.IsAny<String>())).ReturnsAsync((User user, String password) =>
            {
                user.Id = "5";
                return IdentityResult.Success;
            });
            userManagerMock.Setup((mock) => mock.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync("asdfasdf");
            userManager = userManagerMock.Object;

            signInManager = new Mock<SignInManager<User>>(userManager,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<User>>>().Object,
                new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object).Object;
        }

        [Fact]
        public async Task Register_EmailConfirmAsync_CalledOnce()
        {
            var emailSender = new Mock<IEmailSender>();
            emailSender.Setup(mock => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            usersController = new UsersController(userRepo, userManager, signInManager, emailSender.Object, Configuration);
            var newUser = fixture.Create<RegisterViewModel>();

            newUser.Email = "jswann1@wvup.edu";

            await usersController.Register(newUser);

            emailSender.Verify((mock) => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Register_SendEmailThrowsError_Returns400()
        {
            var emailSender = new Mock<IEmailSender>();
            emailSender.Setup(mock => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            usersController = new UsersController(userRepo, userManager, signInManager, emailSender.Object, Configuration);
            var newUser = fixture.Create<RegisterViewModel>();

            var result = await usersController.Register(newUser);
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}