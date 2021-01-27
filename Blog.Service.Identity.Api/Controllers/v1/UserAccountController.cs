using Blog.Service.Identity.Application.Features.UserAccount.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
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

        [HttpPost("password-reset-code")]
        public async Task<IActionResult> GeneratePasswordResetCode([FromBody] GenerateUserAccountPasswordResetTokenCommand request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword([FromBody] UserAccountPasswordResetCommand request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserAccountPasswordCommand request)
        {
            var identity = (ClaimsIdentity)User.Identity;

            // Get the claims values
            var email = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
                               .Select(c => c.Value).SingleOrDefault();

            if (email == null || !email.Equals(request.Email.Trim()))
            {
                return Unauthorized();
            }
            else
            {
                var result = await Mediator.Send(request);

                return Ok(result);
            }
        }
    }
}
