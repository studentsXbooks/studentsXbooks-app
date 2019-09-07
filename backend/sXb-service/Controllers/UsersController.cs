using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sXb_service.Models;
using sXb_service.Services;
using sXb_service.Repos.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using sXb_service.Models.ViewModels;
using System.Text.Encodings.Web;

namespace sXb_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private IUserRepo Repo { get; set; }

        public UsersController( IUserRepo repo,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender)
        {
            Repo = repo;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
        public async Task<IActionResult> Create([FromBody] RegisterViewModel newUser)
        {
            
            User user = new User();
            if (newUser.Username != null)
                user.UserName = newUser.Username;
            else
                return BadRequest();

            if (newUser.Email != null)
                user.Email = newUser.Email;
            else
                return BadRequest();

            var result = await _userManager.CreateAsync(user, newUser.Password);
            
            if (result.Succeeded)
            {
                
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/emailconfirmed",
                    pageHandler: null,
                    values: new { userId = user.Id, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                // Uncomment for registration w/o email confirmation.
                //await _signInManager.SignInAsync(user, isPersistent: false);
               
                return Created($"api/User/Get/{user.Id}", user);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return NotFound();
        }

        [HttpGet("new")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string userId, [FromQuery]string code)
        {
            if (userId == null || code == null)
            {
                return NotFound();
            }
            User user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            
            // TODO: This is a literal url string. Get root url from user secrets.
            return Redirect("http://sxb-front.com:3000/email-confirmed");

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
                // Gets username because login uses username.
                // TODO: Implement custom login method that uses email instead.
                string username = Repo.GetUsernameByEmail(model.Email);
                // This does not count login failures towards account lockout
                // To enable password failures to trigger account lockout,
                // set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    
                    
                    return RedirectToLocal(returnUrl);
                }
            }

            // If execution got this far, something failed, redisplay the form.
            return RedirectToAction(nameof(GetAll));
        }
        [HttpPost("logout")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Ok("Logout complete!");
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
