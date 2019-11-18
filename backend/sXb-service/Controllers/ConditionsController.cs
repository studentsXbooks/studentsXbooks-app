using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sXb_service.Helpers;
using sXb_service.Models;
using sXb_service.Models.ViewModels;

namespace sXb_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConditionsController : ControllerBase
    {
        public ConditionsController() { }

        [HttpGet]
        public IEnumerable<EnumNameValue> GetAll() =>
            EnumExtensions.ToList<Condition>();
    }

}
