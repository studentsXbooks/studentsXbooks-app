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
    }
}