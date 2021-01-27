using System.Linq;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Service.Identity.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/test")]
    [ApiController]
    [Authorize]
    public class TestController : BaseController
    {
        // GET api/values
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Get()
        {
            var identity = (ClaimsIdentity) User.Identity;

            // Get the claims values
            var email = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
                               .Select(c => c.Value).SingleOrDefault();
            return Ok(email);
        }
    }
}
