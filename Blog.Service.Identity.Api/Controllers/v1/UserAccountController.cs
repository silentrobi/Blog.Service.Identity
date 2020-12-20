using Blog.Service.Identity.Application.Features.UserAccount.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Service.Identity.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : BaseController
    {
        public async Task<IActionResult> Register([FromBody] CreateUserAccountCommand request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }
    }
}
