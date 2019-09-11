using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sXb_service.Models;
using sXb_service.Repos.Interfaces;
using sXb_service.ViewModels;

namespace sXb_service.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase {
        private IMapper _mapper;
        private IBookRepo _iRepo;

        public BooksController (IBookRepo iRepo, IMapper mapper) {
            _iRepo = iRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks () => Ok (_iRepo.GetAll ());

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetBook ([FromRoute] Guid id) {
            var book = await _iRepo.Find (id);

            if (book == null) {
                return NotFound ();
            }

            return Ok (book);
        }

        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] BookViewModel bookViewModel) {

            var book = _mapper.Map<Book> (bookViewModel);

            await _iRepo.Add (book);
            return Created ("GetBook", new { id = book.Id });
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> Update ([FromRoute] Guid id, [FromBody] BookViewModel bookViewModel) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            if (id != bookViewModel.Id) {
                return BadRequest ();
            }
            var book = _mapper.Map<Book> (bookViewModel);
            try {
                var result = await _iRepo.Update (book);
                return Ok (result);
            } catch (DbUpdateConcurrencyException) {
                if (!await _iRepo.Exist (id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> Remove ([FromRoute] Guid id) {
            try {
                var result = await _iRepo.Remove (id);
                return Ok (result);
            } catch (DbUpdateConcurrencyException) {
                if (!await _iRepo.Exist (id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }
        }
    }
}