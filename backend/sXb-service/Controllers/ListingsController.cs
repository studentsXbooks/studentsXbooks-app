﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sXb_service.Helpers;
using sXb_service.Helpers.ModelValidation;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.Repos.Interfaces;

namespace sXb_service.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private IMapper _mapper;
        private IListingRepo _iRepo;
        private IBookRepo _iBookRepo;
        private IBookApi _iBookApi;
        private IAuthorRepo _iAuthorRepo;
        private IBookAuthorRepo _iBookAuthorRepo;

        public ListingsController(IListingRepo iRepo, IBookRepo iBookRepo, IAuthorRepo iAuthorRepo, IBookAuthorRepo iBookAuthorRepo, IBookApi iBookApi, UserManager<User> userManager, IMapper mapper)
        {
            _iRepo = iRepo;
            _iBookRepo = iBookRepo;
            _iAuthorRepo = iAuthorRepo;
            _iBookAuthorRepo = iBookAuthorRepo;
            _iBookApi = iBookApi;
            _userManager = userManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetListings([FromQuery] int page = 1)
        {
            var pageResult = new Paging<Listing>(page, _iRepo.GetAll());
            return Ok(pageResult);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListing([FromRoute] Guid id)
        {
            var listing = await _iRepo.Find(x => x.Id == id);

            if (listing == null)
            {
                return NotFound();
            }

            var details = _mapper.Map<ListingDetailsViewModel>(listing);

            return Ok(details);
        }

        [AllowAnonymous]
        [HttpPost("search/{term}/{page}")]
        public IActionResult Search([FromBody] SearchFilter searchFilter, [FromRoute] string term, [FromRoute] int page = 1)
        {
            string query = term.Replace("%20", " ").Replace("+", " ");
            Regex rx = new Regex(@"\b" + query + @"\b", RegexOptions.IgnoreCase);

            // If no conditions selected: set all conditions.
            if (searchFilter.Conditions == null || searchFilter.Conditions.Count() == 0)
            {
                searchFilter.Conditions = new Condition[]
                {
                    Condition.New,
                    Condition.LikeNew,
                    Condition.Good,
                    Condition.Fair,
                    Condition.Poor
                };
            }
            // If no Max Price, Include highest possbile priced item.
            if (searchFilter.MaxPrice == null)
            {
                searchFilter.MaxPrice = int.MaxValue;
            }
            // If no Min Price, Include lowest possible priced item.
            if (searchFilter.MinPrice == null)
            {
                searchFilter.MinPrice = 0;
            }

            // Search compatible with Title, Author, ISBN
            var listing = new Paging<ListingPreviewViewModel>(page,
                _iRepo.GetAll(x =>
                (rx.IsMatch(x.Book.Title) || rx.IsMatch(x.Book.ISBN10) || x.Book.BookAuthors.Any(y => rx.IsMatch(y.Author.FullName) || x.Book.BookAuthors.Any(z => rx.IsMatch(z.Author.FirstName + " " + z.Author.LastName))))
                &&
                (searchFilter.Conditions.Any(y => x.Condition == y) &&
                x.Price >= searchFilter.MinPrice && x.Price <= searchFilter.MaxPrice))
                .Select(x =>
                _mapper.Map<ListingPreviewViewModel>(x)));

            if (listing == null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateListingViewModel createListingViewModel)
        {
            var book = _mapper.Map<Book>(createListingViewModel);
            var author = _mapper.Map<Author>(createListingViewModel);
            var listing = _mapper.Map<Listing>(createListingViewModel);

            if (!await _iBookRepo.Exist(x => x.ISBN10 == book.ISBN10))
            {
                if (!await _iAuthorRepo.Exist(x => x.FullName == author.FullName))
                {
                    await _iAuthorRepo.Create(author);
                    await _iBookRepo.Create(book);

                    var bookAuthor = new BookAuthor()
                    {
                        AuthorId = author.Id,
                        BookId = book.Id
                    };
                    await _iBookAuthorRepo.Create(bookAuthor);
                }
            }
            else
            {
                book = await _iBookRepo.Find(x => x.ISBN10 == book.ISBN10);
            }

            listing.BookId = book.Id;
            var user = await _userManager.GetUserAsync(User);
            listing.UserId = user.Id;
            await _iRepo.Create(listing);

            return Created("GetListing", new { id = listing.Id });
        }


        [HttpGet("user/{page}")]
        public async Task<IActionResult> GetUsersListings(int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pageResult = new Paging<ListingPreviewViewModel>(page, _iRepo.GetAll(x => x.UserId == user.Id).Select(x => _mapper.Map<ListingPreviewViewModel>(x)));
                return Ok(pageResult);
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("find/{term}")]
        public async Task<IActionResult> FindBook(string term, [FromQuery] int page = 1)
        {            
            var books = await _iBookApi.FindBook(term, page);
            return Ok(books);
        }
    }
}
