using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Testing;
using sXb_service;
using sXb_service.Helpers;
using sXb_service.Helpers.ModelValidation;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.ViewModels;
using Xunit;

namespace sXb_tests.Integration {
    [Collection ("Integration")]
    public class ListingsController : IClassFixture<WebApplicationFactory<Startup>> {
        private readonly WebApplicationFactory<Startup> _factory;
        private IFixture fixture;

        public ListingsController (WebApplicationFactory<Startup> factory) {
            _factory = factory;
            fixture = new Fixture ().Customize (new AutoMoqCustomization ());
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookie_Return200WithAPageOfListingVMs () {
            string listingsByUserUrl = "/api/listings/user/1";
            var client = _factory.CreateClient ();
            await client.PostAsJsonAsync<LoginViewModel> ("/api/users/", new LoginViewModel () {
                Email = "test@wvup.edu",
                    Password = "Develop@90"
            });

            var response = await client.GetAsync (listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>> ();
            Assert.Equal (HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull (listings);
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookieAndPageTwo_Return200WithAPageOfListingVMs () {
            string listingsByUserUrl = "/api/listings/user/2";
            var client = _factory.CreateClient ();
            await client.PostAsJsonAsync<LoginViewModel> ("/api/users/", new LoginViewModel () {
                Email = "test@wvup.edu",
                    Password = "Develop@90"
            });

            var response = await client.GetAsync (listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>> ();
            Assert.Equal (HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull (listings);
        }

        [Fact]
        public async Task GetUsersListing_CallerHasCookieAndNoPageData_Return200WithAPageOfNoData () {
            string listingsByUserUrl = "/api/listings/user/1";
            var client = _factory.CreateClient ();
            await client.PostAsJsonAsync<LoginViewModel> ("/api/users/", new LoginViewModel () {
                Email = "nolistingsuser@wvup.edu",
                    Password = "Develop@90"
            });

            var response = await client.GetAsync (listingsByUserUrl);
            var listings = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>> ();
            Assert.Equal (HttpStatusCode.OK, response.StatusCode);
            Assert.Empty (listings.Data);
        }

        [Fact]
        public async Task GetListingDetail_IdExists_Return200WithListingDetailVM () {
            var listing = fixture.Create<ListingViewModel> ();
            var client = _factory.CreateClient ();

            var response = await client.GetAsync ("/api/listings/a059efc3-a4ec-4abf-946a-84194b2e0a00");

            response.EnsureSuccessStatusCode ();
            Assert.Equal (HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetListingDetail_IdNotFound_Return404 () {
            var client = _factory.CreateClient ();

            var response = await client.GetAsync ("/api/listings/81c92cf8-d6c2-4b10-ad00-3eaac26d9b92");

            Assert.Equal (HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ValidListingDetail_Returns201WithModel () {
            string url = "/api/listings/";
            var client = _factory.CreateClient ();
            await client.PostAsJsonAsync<LoginViewModel> ("/api/users/", new LoginViewModel () {
                Email = "test@wvup.edu",
                    Password = "Develop@90"
            });
            var newListing = fixture.Create<CreateListingViewModel> ();
            newListing.Description = "Short book description";
            newListing.FirstName = "Author";
            newListing.MiddleName = "B";
            newListing.LastName = "BookWriter";
            newListing.ISBN10 = "1082148938";
            newListing.Price = 5.99m;
            newListing.Title = "Short Title";
            newListing.UserId = "1234";
            var response = await client.PostAsJsonAsync (url, newListing);

            Assert.Equal (HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData ("This title is way too long and the test should fail because of it because it is really long and it can only be 256 characters but it is longer than that and so it should return a bad request instead of working because this won't work because it is too long and this is a good test so it should not work", "FirstName", "LastName", "middleName", "9780746062760", 4.99, Condition.Fair)]
        [InlineData ("", "FirstName", "LastName", "middleName", "9780746062760", 4.99, Condition.Good)]
        [InlineData ("Normal Length Title", "", "LastName", "middleName", "9780746062760", 4.99, Condition.Fair)]
        [InlineData ("Normal Length Title", "FirstName", "", "middleName", "9780746062760", 4.99, Condition.Fair)]
        [InlineData ("Normal Length Title", "FirstName", "LastName", "", "9780746062760", 4.99, Condition.Fair)]
        [InlineData ("Normal Length Title", "FirstName", "LastName", "MiddleName", "978746062760", 4.99, Condition.Fair)] // Should fail cause ISBN10 not valid
        [InlineData ("Normal Length Title", "FirstName", "LastName", "MiddleName", "978746062760", -4.99, Condition.Fair)]
        public async Task Create_InvalidListingDetail_Return400 (string title, string firstName, string lastName, string middleName, string isbn, decimal price,
            Condition condition) {
            string url = "/api/listings/";
            var client = _factory.CreateClient ();

            await client.PostAsJsonAsync<LoginViewModel> ("/api/users/", new LoginViewModel () {
                Email = "test@wvup.edu",
                    Password = "Develop@90"
            });

            var newListing = new CreateListingViewModel () {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Title = title,
                ISBN10 = isbn,
                Price = price,
                Condition = condition
            };
            var response = await client.PostAsJsonAsync<CreateListingViewModel> (url, newListing);

            var errorMessage = await response.Content.ReadAsAsync<ValidationResultModel> ();
            Assert.Equal (HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.NotNull (errorMessage);
        }
        [Theory]
        [InlineData("Rowling")]
        [InlineData("Joanne+K+Rowling")]
        [InlineData("Foster+Wallace")]
        [InlineData("Melville")]
        [InlineData("Row")]
        public async Task Search_TermIsAnAuthor_Returns200WithPagingWithDataContaingSameAuthor(string query)
        {
            string url = $"/api/listings/search?q={query}";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();



            // Log count of authors reccurence.
            Dictionary<string, int> authorDict = new Dictionary<string, int>();
            

            foreach( ListingPreviewViewModel vm in content.Data)
            {
                foreach( string author in vm.Authors)
                {
                    // if it's a new author initiate with the count of 1
                    int countOfAuthor = 0;
                    try
                    {
                        countOfAuthor = ++authorDict[author];
                        if(authorDict[author] > 0 )
                        {
                            authorDict[author] = countOfAuthor;
                        }
                    }
                    catch(KeyNotFoundException ex)
                    {
                        authorDict.Add(author, 1);
                    }
                }
            }
            bool sameAuthor = false;
            int total;
            
            if (content.TotalRecords > content.PageSize)
            {
                total = content.PageSize;
            }
            else
            {
                total = content.TotalRecords;
            }
            for ( int i = 0; i < authorDict.Count; i++)
            {

                
                // If the authors reccurence is the same as total records count,
                // that means, that at least one author reccurs in all records.
                if( authorDict.ElementAt(i).Value == total )
                {
                    sameAuthor = true;
                    break;
                }
            }

            if (content.TotalRecords > 0)
                Assert.True(sameAuthor);
            else
                Assert.False(sameAuthor);

        }
        [Theory]
        [InlineData("harry+potter")]
        [InlineData("infinite+jest")]
        [InlineData("har")]
        [InlineData("moby")]
        public async Task Search_TermIsATitle_Returns200WithPagingWithDataContainingSameTitle(string query)
        {
            string url = $"/api/listings/search?q={query}";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            // Log count of authors reccurence.
            Dictionary<string, int> authorDict = new Dictionary<string, int>();

            foreach (ListingPreviewViewModel vm in content.Data)
            {
                foreach (string author in vm.Authors)
                {
                    // if it's a new author initiate with the count of 1
                    int countOfAuthor = 0;
                    try
                    {
                        countOfAuthor = ++authorDict[author];
                        if (authorDict[author] > 0)
                        {
                            authorDict[author] = countOfAuthor;
                        }
                    }
                    catch (KeyNotFoundException ex)
                    {
                        authorDict.Add(author, 1);
                    }
                }
            }
            bool sameAuthor = false;
            int total;
            if (content.TotalRecords > content.PageSize)
            {
                total = content.PageSize;
            }
            else
            {
                total = content.TotalRecords;
            }
            for (int i = 0; i < authorDict.Count; i++)
            {
                // If the authors reccurence is the same as total records count,
                // that means, that at least one author reccurs in all records.
                if (authorDict.ElementAt(i).Value == total)
                {
                    sameAuthor = true;
                    break;
                }
            }

            if (content.TotalRecords > 0)
                Assert.True(sameAuthor);
            else
                Assert.False(sameAuthor);
        }
        [Theory]
        [InlineData("9781976530739")]
        [InlineData("123456789")]
        [InlineData("123")]
        public async Task Search_TermIsAnISBN_Returns200WithPagingWithDataContainingSameISBN(string query)
        {
            string url = $"/api/listings/search?q={query}";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            // Log count of authors reccurence.
            Dictionary<string, int> authorDict = new Dictionary<string, int>();

            foreach (ListingPreviewViewModel vm in content.Data)
            {
                foreach (string author in vm.Authors)
                {
                    // if it's a new author initiate with the count of 1
                    int countOfAuthor = 0;
                    try
                    {
                        countOfAuthor = ++authorDict[author];
                        if (authorDict[author] > 0)
                        {
                            authorDict[author] = countOfAuthor;
                        }
                    }
                    catch (KeyNotFoundException ex)
                    {
                        authorDict.Add(author, 1);
                    }
                }
            }
            bool sameAuthor = false;
            int total;
            if (content.TotalRecords > content.PageSize)
            {
                total = content.PageSize;
            }
            else
            {
                total = content.TotalRecords;
            }
            for (int i = 0; i < authorDict.Count; i++)
            {
                // If the authors reccurence is the same as total records count,
                // that means, that at least one author reccurs in all records.
                if (authorDict.ElementAt(i).Value == total)
                {
                    sameAuthor = true;
                    break;
                }
            }

            if (content.TotalRecords > 0)
                Assert.True(sameAuthor);
            else
                Assert.False(sameAuthor);
        }
        [Theory]
        [InlineData("9781976530739")]
        [InlineData("harry+potter")]
        [InlineData("Joanne+K+Rowling")]
        public async Task Search_TermIs_X__Returns200WithPagingWithDataContaining_X_InAuthorOrTitleOrISBN(string query)
        {
            string url = $"/api/listings/search?q={query}";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            // Log count of authors reccurence.
            Dictionary<string, int> authorDict = new Dictionary<string, int>();

            foreach (ListingPreviewViewModel vm in content.Data)
            {
                foreach (string author in vm.Authors)
                {
                    // if it's a new author initiate with the count of 1
                    int countOfAuthor = 0;
                    try
                    {
                        countOfAuthor = ++authorDict[author];
                        if (authorDict[author] > 0)
                        {
                            authorDict[author] = countOfAuthor;
                        }
                    }
                    catch (KeyNotFoundException ex)
                    {
                        authorDict.Add(author, 1);
                    }
                }
            }
            bool sameAuthor = false;
            int total;
            if (content.TotalRecords > content.PageSize)
            {
                total = content.PageSize;
            }
            else
            {
                total = content.TotalRecords;
            }
            for (int i = 0; i < authorDict.Count; i++)
            {
                // If the authors reccurence is the same as total records count,
                // that means, that at least one author reccurs in all records.
                if (authorDict.ElementAt(i).Value == total)
                {
                    sameAuthor = true;
                    break;
                }
            }

            if (content.TotalRecords > 0)
                Assert.True(sameAuthor);
            else
                Assert.False(sameAuthor);
        }
        [Theory]
        [InlineData("infinite+jest")]
        [InlineData("infinite")]
        public async Task Search_TermHasManyPages_Returns200NextIsTrue(string query)
        {
            string url = $"/api/listings/search?q={query}";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

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
            string url = $"/api/listings/search?q={query}";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

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
            string url = $"/api/listings/search?q={query}";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Paging<ListingPreviewViewModel> content = await response.Content.ReadAsAsync<Paging<ListingPreviewViewModel>>();

            Assert.Equal(0, content.TotalRecords);
        }
    }
}
