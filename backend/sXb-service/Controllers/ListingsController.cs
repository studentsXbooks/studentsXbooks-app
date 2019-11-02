using System;
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
using sXb_service.Services;
using sXb_service.ViewModels;

namespace sXb_service.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IListingRepo _iListingRepo;
        private readonly IBookRepo _iBookRepo;
        private readonly IAuthorRepo _iAuthorRepo;
        private readonly IBookAuthorRepo _iBookAuthorRepo;

        public ListingsController(IListingRepo iRepo,
            IBookRepo iBookRepo,
            IAuthorRepo iAuthorRepo,
            IBookAuthorRepo iBookAuthorRepo,
            UserManager<User> userManager,
            IMapper mapper,
            IEmailSender emailSender)
        {
            _iListingRepo = iRepo;
            _iBookRepo = iBookRepo;
            _iAuthorRepo = iAuthorRepo;
            _iBookAuthorRepo = iBookAuthorRepo;
            _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetListings([FromQuery] int page = 1)
        {
            var pageResult = new Paging<Listing>(page, _iListingRepo.GetAll());
            return Ok(pageResult);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListing([FromRoute] Guid id)
        {
            var listing = await _iListingRepo.Find(x => x.Id == id);

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
                _iListingRepo.GetAll(x =>
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
            await _iListingRepo.Create(listing);

            return Created("GetListing", new { id = listing.Id });
        }


        [HttpGet("user/{page}")]
        public async Task<IActionResult> GetUsersListings(int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pageResult = new Paging<ListingPreviewViewModel>(page, _iListingRepo.GetAll(x => x.UserId == user.Id).Select(x => _mapper.Map<ListingPreviewViewModel>(x)));
                return Ok(pageResult);
            }
            return NotFound();
        }

        [HttpPost("contact")]
        [AllowAnonymous]
        public async Task<IActionResult> Contact([FromBody] ContactViewModel contact)
        {
            var listing = await _iListingRepo.Find(x => x.Id.Equals(contact.ListingId));
            var defaultSubjectMessage = $"{contact.Email} is interested in your book!";
            var body = contact.Body + "\n\nReply to this email to follow if you like this offer.";
            if (!(listing is null))
            {
                switch (listing.ContactOption)
                {
                    case ContactOption.SellerContactBuyer:
                        _emailSender.SendEmailAsync(listing.User.Email, contact.Email, defaultSubjectMessage, contact.Body);
                        return Created("/listings/1", contact);
                    default:
                        return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet("contact")]
        [AllowAnonymous]
        public IActionResult Contact()
            => Ok(EnumExtensions.ToList<ContactOption>());
    }
}
