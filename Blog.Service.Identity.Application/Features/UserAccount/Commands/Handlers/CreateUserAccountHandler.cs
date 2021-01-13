using Blog.Service.Identity.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands.Handlers
{
    public class CreateUserAccountHandler : IRequestHandler<CreateUserAccountCommand, bool>
    {
        private readonly UserManager<User> _userManager;

        public CreateUserAccountHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(CreateUserAccountCommand request, CancellationToken cancellationToken)
        {
            var user = new User() { 
                UserName  = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync( user , request.Password);

            if (!result.Succeeded) throw new Exceptions.ApplicationException(result.Errors.ToString());

            // Add some user claims. Note these claims get embedded in the tokens returned to the client during the authentication phase.

            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", "Consumer"));

            return true;
        }
    }
}
