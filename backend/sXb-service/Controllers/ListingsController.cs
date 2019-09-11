using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.Repos.Interfaces;
using sXb_service.ViewModels;

namespace sXb_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private IMapper _mapper;
        private IListingRepo _iRepo;

        public ListingsController(IListingRepo iRepo, UserManager<User> userManager, IMapper mapper)
        {
            _iRepo = iRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetListings() => Ok(_iRepo.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetListing([FromRoute] Guid id)
        {
            var listing = await _iRepo.Find(id);

            if (listing == null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListingViewModel listingViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listing = _mapper.Map<Listing>(listingViewModel);

            await _iRepo.Add(listing);
            return Created("GetListing", new { id = listing.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ListingViewModel listingViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != listingViewModel.Id)
            {
                return BadRequest();
            }
            var listing = _mapper.Map<Listing>(listingViewModel);
            try
            {
                var result = await _iRepo.Update(listing);
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _iRepo.Exist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            try
            {
                var result = await _iRepo.Remove(id);
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _iRepo.Exist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUsersListings()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                var listings = await _iRepo.ByUser(user.Id);
                return Ok(listings.Select(x => _mapper.Map<ListingDetailsViewModel>(x)));
            }
            return NotFound();
        }
    }
}