using Blog.Service.Identity.Application.Features.UserAccount.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Service.Identity.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/identity/users")]
    public class UserAccountController : BaseController
    {
        public UserAccountController()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserAccountCommand request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }
    }
}
