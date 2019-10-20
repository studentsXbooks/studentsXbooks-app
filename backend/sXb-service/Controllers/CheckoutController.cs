using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using sXb_service.Models;
using sXb_service.Repos.Interfaces;
using sXb_service.Helpers;

namespace sXb_service.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private IListingRepo _iRepo;
        private StripeConfig _stripeConfig;
        private IUserRepo _iUserRepo;
        public IConfiguration Configuration { get; }

        public CheckoutController(IListingRepo iRepo, IUserRepo iUserRepo, UserManager<User> userManager, IConfiguration configuration, StripeConfig stripeConfig)
        {
            _iRepo = iRepo;
            _iUserRepo = iUserRepo;
            _userManager = userManager;
            Configuration = configuration;
            _stripeConfig = stripeConfig;
        }

        public async Task<ActionResult> CreateCharge([FromBody] ListingCharge listingCharge)
        {
            StripeConfiguration.ApiKey = _stripeConfig.Apikey;
            var chargeService = new ChargeService();
            var listing = await _iRepo.Find(x => x.Id == listingCharge.ListingId);
            var user = await _iUserRepo.Get(listing.UserId);

            ChargeCreateOptions options = new ChargeCreateOptions {
                Amount = (long)listingCharge.Amount,
                Currency = "usd",
                Source = listingCharge.BuyerToken,
                OnBehalfOf = user.StripeUserId
            };

            Charge charge = chargeService.Create(options);

            if (charge.Status == "succeeded")
            {
                listing.Status = Status.Purchased;

                return Ok();
            }
            return BadRequest();
        }

        public async Task<ActionResult> LinkStripeAccount(StripeUserAccess stripeUserAccess)
        {
            var user = await _userManager.GetUserAsync(User);
            user.StripeUserId = stripeUserAccess.Code;
            await _iUserRepo.Update(user);

            return Ok();
        }
        
        public async Task<ActionResult> HasLinkedStripe(StripeUserAccess stripeUserAccess)
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(user.StripeUserId == null);
        }
    }
}