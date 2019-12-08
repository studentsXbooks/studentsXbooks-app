using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sXb_service.Helpers;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.Repos.Interfaces;
using sXb_service.Services;

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

        [Authorize]
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel newUser)
        {

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

            if (!Regex.Match(newUser.Email, ".+@.+[.]\\w").Success)
            {
                return BadRequest(new ErrorMessage("Invalid email address."));
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
                var corsConfig = Configuration.GetSection("Cors").Get<CorsConfig>();
                string frontendUrl = corsConfig.FrontendDomain;
                _emailSender.SendEmailAsync(user.Email, "Confirm your email", string.Format("Please confirm your account by <a href='{0}/confirm-email?id={1}&code={2}'>clicking here</a>.", frontendUrl, HttpUtility.UrlEncode(user.Id), HttpUtility.UrlEncode(code)));

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
                return Ok(new { accountConfirm = true });
            return BadRequest();
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
                string username = null;
                try
                {
                    username = await Repo.GetUsernameByEmail(model.Email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return NotFound();
                }

                // This does not count login failures towards account lockout
                // To enable password failures to trigger account lockout,
                // set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    return Ok();
                }
            }

            // If execution got this far, something failed, redisplay the form.
            return NotFound();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                return Ok("Logout complete!");
            }
            return BadRequest();
        }
    }
}