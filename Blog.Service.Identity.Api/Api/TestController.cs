using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4Demo.Api
{
    [Route("/api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Authorize(policy:"apiread")]
        public IActionResult Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            
            // Get the claims values
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            var test = identity.Claims.ToList();
            return new JsonResult(claims);
        }
    }
}