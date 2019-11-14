using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.Testing;
using sXb_service;
using sXb_service.Helpers;
using sXb_service.Helpers.ModelValidation;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.ViewModels;
using Xunit;

namespace sXb_tests.Integration
{
    [Collection("Integration")]
    public class ListingsController : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;

        public ListingsController(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookie_Return200WithAPageOfListingVMs()
        {
            string listingsByUserUrl = "/api/listings/user/1";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });

            var response = await client.GetAsync(listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(listings);
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookieAndPageTwo_Return200WithAPageOfListingVMs()
        {
            string listingsByUserUrl = "/api/listings/user/2";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });

            var response = await client.GetAsync(listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(listings);
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookieAndNoPageData_Return200WithAPageOfNoData()
        {
            string listingsByUserUrl = "/api/listings/user/1";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "nolistingsuser@wvup.edu",
                Password = "Develop@90"
            });

            var response = await client.GetAsync(listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(listings.Data);
        }

        [Fact]
        public async Task GetListingDetail_IdExists_Return200WithListingDetailVM()
        {
            var listing = fixture.Create<ListingViewModel>();
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/listings/a059efc3-a4ec-4abf-946a-84194b2e0a00");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetListingDetail_IdNotFound_Return404()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/listings/81c92cf8-d6c2-4b10-ad00-3eaac26d9b92");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ValidListingDetail_Returns201WithModel()
        {
            string url = "/api/listings/";
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });
            var newListing = fixture.Create<CreateListingViewModel>();
            newListing.Description = "Short book description";
            newListing.Authors = "Author One, Author Two";
            newListing.ISBN10 = "1082148938";
            newListing.ISBN13 = "9780201616224";
            newListing.Price = 5.99m;
            newListing.Title = "Short Title";
            newListing.UserId = "1234";
            var response = await client.PostAsJsonAsync(url, newListing);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("This title is way too long and the test should fail because of it because it is really long and it can only be 256 characters but it is longer than that and so it should return a bad request instead of working because this won't work because it is too long and this is a good test so it should not work", "Normal Author",  "9780746062760", "9780201616224", 4.99, Condition.Fair)]
        [InlineData("", "Normal Author",  "9780746062760", "9780201616224", 4.99, Condition.Good)]
        [InlineData("Normal Length Title", "",  "9780746062760", "9780201616224", 4.99, Condition.Fair)]
        [InlineData("Normal Length Title", "Normal Author, Second Author",  "978746062760", "9780201616224", 4.99, Condition.Fair)] // Should fail cause ISBN10 not valid
        [InlineData("Normal Length Title", "Normal Author, Second Author",  "978746062760", "9780201655555554", -4.99, Condition.Fair)] // Should fail cause ISBN13 not valid
        public async Task Create_InvalidListingDetail_Return400(string title, string authors,  string isbn10, string isbn13, decimal price,
            Condition condition)
        {
            string url = "/api/listings/";
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync<LoginViewModel>("/api/users/", new LoginViewModel()
            {
                Email = "test@wvup.edu",
                Password = "Develop@90"
            });

            var newListing = new CreateListingViewModel()
            {
                Authors = authors,
                Title = title,
                ISBN10 = isbn10,
                ISBN13 = isbn13,
                Price = price,
                Condition = condition
            };
            var response = await client.PostAsJsonAsync<CreateListingViewModel>(url, newListing);

            var errorMessage = await response.Content.ReadAsAsync<ValidationResultModel>();
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.NotNull(errorMessage);
        }

        [Theory]
        [InlineData("Rowling")]
        [InlineData("Joanne k rowling")]
        [InlineData("foster wallace")]
        [InlineData("melville")]
        public async Task Search_TermIsAnAuthor_Returns200WithPagingWithDataContaingSameAuthor(string term)
        {

            string url = $"/api/listings/search/{term}/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync(url, new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            var isSameAuthor = content.Data.All(x => x.Authors.Any(a => a.ToLower().Contains(term.ToLower())));
            Assert.True(isSameAuthor);
        }

        [Theory]
        [InlineData("harry potter")]
        [InlineData("infinite jest")]
        [InlineData("moby")]
        public async Task Search_TermIsATitle_Returns200WithPagingWithDataContainingSameTitle(string term)
        {
            string url = $"/api/listings/search/{term}/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync(url, new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            var isSameTitle = content.Data.All(x => x.Title.ToLower().Contains(term));
            Assert.True(isSameTitle);
        }

        [Theory]
        [InlineData("9781976530739")]
        [InlineData("123456789")]
        [InlineData("123")]
        public async Task Search_TermIsAnISBN_Returns200WithPagingWithDataContainingSameISBN(string term)
        {
            string url = $"/api/listings/search/{term}/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync(url, new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            var isSameISBN = content.Data.All(x => x.ISBN10.Contains(term));
            Assert.True(isSameISBN);
        }

        [Theory]
        [InlineData("9781976530739")]
        [InlineData("harry potter")]
        [InlineData("Joanne K Rowling")]
        public async Task Search_TermIs_X__Returns200WithPagingWithDataContaining_X_InAuthorOrTitleOrISBN(string term)
        {
            string url = $"/api/listings/search/{term}/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync(url, new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            var isSameX = content.Data.All(x => x.ISBN10.Contains(term) || x.Title.ToLower().Contains(term) || x.Authors.Any(y => y.ToLower().Contains(term.ToLower())));
            Assert.True(isSameX);
        }

        [Theory]
        [InlineData("infinite+jest")]
        [InlineData("infinite")]
        public async Task Search_TermHasManyPages_Returns200NextIsTrue(string query)
        {
            string url = $"/api/listings/search/{query}/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync(url, new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            Assert.True(content.HasNext);
        }

        [Theory]
        [InlineData("9781976530739")]
        [InlineData("harry+potter")]
        [InlineData("Joanne+K+Rowling")]
        public async Task Search_TermHasOnePage_Returns200NextIsFalse(string query)
        {
            string url = $"/api/listings/search/{query}/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync(url, new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            Assert.False(content.HasNext);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("har")]
        [InlineData("joan")]
        public async Task Search_TermHasNoMatches_Returns200PageWithNoDataPrevIsFalseAndNextIsFalse(string query)
        {
            string url = $"/api/listings/search/{query}/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync(url, new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            Assert.Equal(0, content.TotalRecords);
            Assert.False(content.HasNext);
            Assert.False(content.HasPrev);
        }





        [Fact]
        public async Task SearchFilter_NoConditions_Return200_and_ResultsHaveAnyCondition()
        {
            SearchFilter sfilter = new SearchFilter();
            string url = $"/api/listings/search/infinite+jest/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<SearchFilter>(url, sfilter);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            bool isAnyCondition = content.Data.All(x => Enum.GetNames(typeof(Condition)).Any(c => c == x.Condition));

            Assert.True(isAnyCondition);
        }
        [Fact]
        public async Task SearchFilter_OneConditionSelected_Returns200_and_AllResultsMatchCondition()
        {
            SearchFilter sfilter = new SearchFilter();
            sfilter.Conditions = new Condition[]
            {
                Condition.Good
            };
            string url = $"/api/listings/search/infinite+jest/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<SearchFilter>(url, sfilter);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            bool isOneCondition = content.Data.All(x => x.Condition == Enum.GetName(typeof(Condition), Condition.Good));

            Assert.True(isOneCondition);
        }
        [Fact]
        public async Task SearchFilter_TwoConditionsSelected_Return200_and_AllResultsMatchACondition()
        {
            SearchFilter sfilter = new SearchFilter();
            sfilter.Conditions = new Condition[]
            {
                Condition.Good,
                Condition.Fair
            };
            string url = $"/api/listings/search/infinite+jest/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<SearchFilter>(url, sfilter);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            bool isTwoConditions = content.Data.All(x => sfilter.Conditions.Any(a => Enum.GetName(typeof(Condition), a) == x.Condition));

            Assert.True(isTwoConditions);
        }
        [Fact]
        public async Task SearchFilter_MinPriceIs5_Return200_and_AllResultsPricesAreEqualToOrAboveMinPrice()
        {
            SearchFilter sfilter = new SearchFilter();
            sfilter.MinPrice = 5;

            string url = $"/api/listings/search/infinite+jest/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<SearchFilter>(url, sfilter);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            bool isAboveMinPrice = content.Data.All(x => x.Price >= sfilter.MinPrice);

            Assert.True(isAboveMinPrice);
        }
        [Fact]
        public async Task SearchFilter_MaxPriceIs10_Return200_and_AllResultsPricesAreEqualToOrBelowMaxzPricxe()
        {
            SearchFilter sfilter = new SearchFilter();
            sfilter.MaxPrice = 10;

            string url = $"/api/listings/search/infinite+jest/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<SearchFilter>(url, sfilter);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            bool isBelowMaxPrice = content.Data.All(x => x.Price <= sfilter.MaxPrice);

            Assert.True(isBelowMaxPrice);
        }
        [Fact]
        public async Task SearchFilter_FilterOnAll_Return200_and_AllResultsMatchFilter()
        {
            SearchFilter sfilter = new SearchFilter();
            sfilter.Conditions = new Condition[]
            {
                Condition.Good, Condition.Fair
            };
            sfilter.MinPrice = 25;
            sfilter.MaxPrice = 50;

            string url = $"/api/listings/search/infinite+jest/1";
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<SearchFilter>(url, sfilter);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            bool isPerfectMatch = content.Data.All(x => x.Price >= sfilter.MinPrice && x.Price <= sfilter.MaxPrice && (sfilter.Conditions.Any(a => Enum.GetName(typeof(Condition), a) == x.Condition)));


            Assert.True(isPerfectMatch);
        }

        [Fact]
        public async void Contact_ContactDoesNotHaveBody_ShouldReturn422()
        {
            var contact = new ContactViewModel()
            {
                Email = "me@yahoo.com",
                Body = "",
                ListingId = Guid.Parse("a059efc3-a4ec-4abf-946a-84194b2e0a00")
            };
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<ContactViewModel>("api/listings/contact", contact);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async void Contact_ContactDoesNotHaveListingId_ShouldReturn422()
        {
            var contact = new ContactViewModel()
            {
                Email = "me@yahoo.com",
                Body = "Hey,dude I wanna get your book",
            };
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<ContactViewModel>("api/listings/contact", contact);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async void Contact_ListingIdDoesNotExist_ShouldReturn400()
        {
            var contact = new ContactViewModel()
            {
                Email = "me@yahoo.com",
                Body = "Hey,dude I wanna get your book",
                ListingId = Guid.NewGuid() // A random guid could exist but highly unlikely
            };
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync<ContactViewModel>("api/listings/contact", contact);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void Contact_GET_ShouldReturn200AndAllContactOptionsEnums()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/listings/contact");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contactOptionsFromApi = await response.Content.ReadAsAsync<IEnumerable<EnumNameValue>>();
            var contactOptionEnums = EnumExtensions.GetValues<ContactOption>();

            Assert.NotEmpty(contactOptionsFromApi);
            // toString returns back name of enum
            Assert.Equal(contactOptionEnums.Select(x => x.ToString()), contactOptionsFromApi.Select(x => x.Name));
            // casting Enum to int returns back underlying value
            Assert.Equal(contactOptionEnums.Cast<int>(), contactOptionsFromApi.Select(x => x.Value));
        }
    }
}