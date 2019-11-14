using System.Security.Claims;
using System.Linq.Expressions;
using System;
using sXb_service.Controllers;
using Xunit;
using Moq;
using sXb_service.Repos.Interfaces;
using AutoMapper;
using sXb_tests.Helpers;
using sXb_service.ViewModels;
using Microsoft.AspNetCore.Mvc;
using sXb_service.Models;
using sXb_service.Services;
using Microsoft.AspNetCore.Identity;

namespace sXb_tests.Controllers
{
    public class ListingControllerTest
    {

        public ListingsController listingsAPI;
        public Mock<IEmailSender> emailSender;
        public Mock<IListingRepo> listingRepo;

        public ListingControllerTest()
        {
            listingRepo = new Mock<IListingRepo>();
            var bookRepo = new Mock<IBookRepo>();
            var authorRepo = new Mock<IAuthorRepo>();
            var bookAuthorRepo = new Mock<IBookAuthorRepo>();
            var userManager = MockIdentity.UserManagerMock();
            var mapper = new Mock<IMapper>();
            emailSender = new Mock<IEmailSender>();

            listingsAPI = new ListingsController(listingRepo.Object, bookRepo.Object, authorRepo.Object, bookAuthorRepo.Object, userManager.Object, mapper.Object);
        }

        [Fact]
        public async void Contact_ListingHasSELLERCONTACTBUYERChosen_ShouldCallSendEmailOnceAnd201()
        {
            var sellerEmail = "seller@gmail.com";
            var buyerEmail = "buyer@wvup.edu";
            var listingId = Guid.NewGuid();
            var contact = new ContactViewModel()
            {
                Body = "HEY, I want to buy your book",
                ListingId = listingId,
                Email = buyerEmail
            };
            listingRepo.Setup(x => x.Find(It.IsAny<Expression<Func<Listing, Boolean>>>())).ReturnsAsync(new Listing()
            {
                ContactOption = ContactOption.SellerContactBuyer,
                User = new User()
                {
                    Email = sellerEmail
                }
            });

            var result = await listingsAPI.Contact(contact);

            Assert.IsType<CreatedResult>(result);
            emailSender.Verify((a) => a.SendEmailAsync(It.Is<string>(x => x == sellerEmail), It.Is<string>(x => x == buyerEmail), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
        }

        [Fact]
        public async void Contact_ListingDoesNotHaveSELLERCONTACTBUYERChoosen_ShouldReturn400()
        {
            var listingId = Guid.NewGuid();
            var contact = new ContactViewModel()
            {
                Body = "HEY, I want to buy your book",
                ListingId = listingId
            };
            listingRepo.Setup(x => x.Find(It.IsAny<Expression<Func<Listing, Boolean>>>())).ReturnsAsync(new Listing()
            {
                ContactOption = ContactOption.InternalMessaging,
                User = new User()
                {
                    Email = "fakeuser@gmail.com"
                }
            });
            var result = await listingsAPI.Contact(contact);
            Assert.IsType<BadRequestResult>(result);
            emailSender.Verify((a) => a.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
        }
    }
}