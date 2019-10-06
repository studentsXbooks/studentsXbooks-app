using System.Web;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.Repos.Interfaces;
using sXb_service.Services;
using System.Text.RegularExpressions;
using sXb_service.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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
        public IConfiguration Configuration { get; }

        public UsersController(IUserRepo repo,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender, IConfiguration configuration)
        {
            Repo = repo;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            Configuration = configuration;
        }

        //http://localhost:40001/api/[controller]/
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<User> data = Repo.GetAll();
            return data == null ? (IActionResult)NotFound() : new ObjectResult(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            User item = await Repo.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }
        [HttpGet("find-id-by-email/{email}")]
        public async Task<IActionResult> FindIdByEmail(string email)
        {
            string id = await Repo.FindIdByEmail(email);
            if (id == null)
            {
                return NotFound();
            }
            return Ok(id);
        }
        [HttpGet("find-id-by-username/{username}")]
        public async Task<IActionResult> FindIdByUsername(string username)
        {
            string id = await Repo.FindIdByUsername(username);
            if (id == null)
            {
                return NotFound();
            }
            return Ok(id);
        }
        [HttpGet("name")]
        public async Task<IActionResult> GetUsername()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return BadRequest(new ErrorMessage("No cookie found for user."));
            }
            return Ok(new { username = user.UserName });
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

<<<<<<< HEAD
        [HttpPost ("register")]
        public async Task<IActionResult> Register ([FromBody] RegisterViewModel newUser) {

            User user = new User ();
=======
        [HttpPost("new")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel newUser)
        {
>>>>>>> 293f73cea067b88fe516e01c2d002b793ec192f2

            User user = new User();

            if (newUser.Password == null)
            {
                return BadRequest(new ErrorMessage("Password cannot be empty."));
            }

            if (newUser.Username != null)
                user.UserName = newUser.Username;
            else
                return BadRequest();

            // Validate: username doesn't already exist.
            if (await Repo.UsernameExists(newUser.Username))
            {
                return BadRequest(new ErrorMessage("Username already taken!"));
            }

            if (newUser.Email != null)
                user.Email = newUser.Email;
            else
                return BadRequest();

            // Validate: .edu email address.
            if (!Regex.Match(newUser.Email, ".+@.+[.]edu").Success)
            {
                return BadRequest(new ErrorMessage("Invalid email address: Not an edu email address."));
            }
            // Validate: email doesn't already exist.
            if (await Repo.EmailExists(newUser.Email))
            {
                return BadRequest(new ErrorMessage("Email already exists!"));
            }

            // Now, create user.
            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (result.Succeeded)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                string frontendUrl = Configuration["Domain:sXb-frontend"];
                _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    string.Format("Please confirm your account by <a href='{0}/confirm-email?id={1}&code={2}'>clicking here</a>.", frontendUrl, HttpUtility.UrlEncode(user.Id), HttpUtility.UrlEncode(code)));

                return Created($"api/users/{user.Id}", new { Id = user.Id, Code = HttpUtility.UrlEncode(code) });
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

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string id, [FromQuery] string code)
        {
            if (id == null || code == null)
            {
                return NotFound();
            }
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {

                //throw new ApplicationException ($"Unable to load user with ID '{userId}'.");
                return BadRequest();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }

        [HttpGet("Search/{keyword}")]
        public async Task<IActionResult> Search(string keyword)
        {
            var users = Repo.FindUsers(keyword);
            return Ok(users);
        }

        // TODO: Pattern mat
        [HttpGet("{first}/{last}")]
        public async Task<IActionResult> FindIdByName(string first, string last)
        {
            string data = await Repo.FindIdByName(first, last);
            return data == null ? (IActionResult)NotFound() : new ObjectResult(data);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // Gets username because login uses username.
                // TODO: Implement custom login method that uses email instead.
<<<<<<< HEAD
                string username = null;
                try
                {
                    username = await Repo.GetUsernameByEmail(model.Email);
                } catch(Exception ex)
                {
                    return NotFound();
                }
=======
                string username = await Repo.GetUsernameByEmail(model.Email);
>>>>>>> 293f73cea067b88fe516e01c2d002b793ec192f2
                // This does not count login failures towards account lockout
                // To enable password failures to trigger account lockout,
                // set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

<<<<<<< HEAD
                    return Ok();
=======
                    return RedirectToLocal(returnUrl);
>>>>>>> 293f73cea067b88fe516e01c2d002b793ec192f2
                }
            }

            // If execution got this far, something failed, redisplay the form.
<<<<<<< HEAD
            return NotFound();
=======
            return RedirectToAction(nameof(GetId));
>>>>>>> 293f73cea067b88fe516e01c2d002b793ec192f2
        }
        [HttpGet("id")]
        //[Authorize]
        public async Task<IActionResult> GetId()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return BadRequest(new ErrorMessage("No cookie found for user."));
            }
            return Ok(new { Id = user.Id });
        }

        [HttpPost("logout")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok("Logout complete!");
        }
<<<<<<< HEAD
=======

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
>>>>>>> 293f73cea067b88fe516e01c2d002b793ec192f2
    }
}
