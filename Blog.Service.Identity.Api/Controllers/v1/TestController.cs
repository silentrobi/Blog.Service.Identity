using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Service.Identity.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/test")]
    [ApiController]
    public class TestController : BaseController
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var userClaims = User.Claims.ToList();

            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }).ToList());
        }
    }
}
