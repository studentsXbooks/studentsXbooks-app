using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sXb_service.Helpers;
using sXb_service.Helpers.ModelValidation;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.Repos.Interfaces;
using sXb_service.ViewModels;

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
        private IAuthorRepo _iAuthorRepo;
        private IBookAuthorRepo _iBookAuthorRepo;

        public ListingsController(IListingRepo iRepo, IBookRepo iBookRepo, IAuthorRepo iAuthorRepo, IBookAuthorRepo iBookAuthorRepo, UserManager<User> userManager, IMapper mapper)
        {
            _iRepo = iRepo;
            _iBookRepo = iBookRepo;
            _iAuthorRepo = iAuthorRepo;
            _iBookAuthorRepo = iBookAuthorRepo;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetListings([FromQuery] int page = 1)
        {
            var pageResult = new Paging<Listing>(page, _iRepo.GetAll());
            return Ok(pageResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetListing([FromRoute] Guid id)
        {
            var listing = await _iRepo.Find(x => x.Id == id);

            if (listing == null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListingDetailsViewModel listingDetailsViewModel)
        {
            var book = _mapper.Map<Book>(listingDetailsViewModel);
            var author = _mapper.Map<Author>(listingDetailsViewModel);
            var listing = _mapper.Map<Listing>(listingDetailsViewModel);

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
            await _iRepo.Create(listing);

            return Created("GetListing", new { id = listing.Id });
        }


        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUsersListings(int page)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pageResult = new Paging<Listing>(page, _iRepo.GetAll(x => x.UserId == user.Id));
                var listings = pageResult.Data.Select(x => _mapper.Map<ListingDetailsViewModel>(x));
                return Ok(listings);
            }
            return NotFound();
        }
    }
}