using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sXb_service.Models;
using sXb_service.Models.AccountViewModels;
using sXb_service.Services;
using sXb_service.Repos.Interfaces;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace sXb_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private IUserRepo Repo { get; set; }

        public UsersController( IUserRepo repo,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            Repo = repo;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }


        
        //http://localhost:40001/api/[controller]/
        [HttpGet]
        public IActionResult GetAll ()
        {
            IEnumerable<User> data = Repo.GetAll();
            return data == null ? (IActionResult)NotFound() : new ObjectResult(data);
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var item = Repo.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }
        [HttpGet("name")]
        public async Task<IActionResult>GetUsername()
        {
            return Ok(new { username = (await _userManager.GetUserAsync(HttpContext.User)).UserName });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] User item)
        {
            User user = await Repo.Get(id);
            if (item == null || item.Id != id || !ModelState.IsValid)
            {
                return BadRequest();
            }
            if (item.FirstName != null)
            {
                user.FirstName = item.FirstName;
            }
            if (item.LastName != null)
            {
                user.LastName = item.LastName;
            }
            
            if (item.Email != null)
            {
                user.Email = item.Email;
            }
            Repo.Update(user);

            return RedirectToAction("GetAll");
        }

        //TODO: Create Viewmodel for user password, don't pass in url.
        [HttpPost("new")]
        public async Task<IActionResult> Create([FromBody] LoginViewModel login)
        {
            User user = new User();
            if (login.Email != null)
                user.UserName = login.Email;
            if (login.Email != null)
                user.Email = login.Email;

            var result = await _userManager.CreateAsync(user, login.Password);
            
            if (result.Succeeded)
            {
                return Created($"api/User/Get/{user.Id}", user);
            }

            return NotFound();
        }

        [HttpGet("Search/{keyword}")]
        public IActionResult Search(string keyword)
        {
            var users = Repo.FindUsers(keyword);
            return Ok(users);
        }

        // TODO: Pattern mat
        [HttpGet("{first}/{last}")]
        public IActionResult FindIdByName(string first, string last)
        {
            string data = Repo.FindIdByName(first, last);
            return data == null ? (IActionResult)NotFound() : new ObjectResult(data);
        }
        

       

        [HttpPost]
        [AllowAnonymous]
        //TODO: Enable for Anti-XSRF
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This does not count login failures towards account lockout
                // To enable password failures to trigger account lockout,
                // set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
            }

            // If execution got this far, something failed, redisplay the form.
            return RedirectToAction(nameof(GetAll));
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                // Questionable to return all records.
                return RedirectToAction(nameof(GetAll));
            }
        }
    }
}
