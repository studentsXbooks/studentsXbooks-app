using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sXb_service.Models;
using sXb_service.Repos.Interfaces;
using sXb_service.ViewModels;

namespace sXb_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBooksController : ControllerBase
    {
        private IMapper _mapper;
        private IUserBookRepo _iRepo;

        public UserBooksController(IUserBookRepo iRepo, IMapper mapper)
        {
            _iRepo = iRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetListings() => Ok(_iRepo.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserBook([FromRoute] Guid id)
        {
            var userBook = await _iRepo.Find(id);

            if (userBook == null)
            {
                return NotFound();
            }

            return Ok(userBook);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserBookViewModel userBookViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userBook = _mapper.Map<UserBook>(userBookViewModel);

            await _iRepo.Add(userBook);
            return Created("GetUserBook", new { id = userBook.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserBookViewModel userBookViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userBookViewModel.Id)
            {
                return BadRequest();
            }
            var userBook = _mapper.Map<UserBook>(userBookViewModel);
            try
            {
                var result = await _iRepo.Update(userBook);
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
    }
}