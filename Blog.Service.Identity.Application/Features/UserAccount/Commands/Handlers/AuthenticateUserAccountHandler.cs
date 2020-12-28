using Blog.Service.Identity.Application.Features.UserAccount.Services;
using Blog.Service.Identity.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands.Handlers
{
    public class AuthenticateUserAccountHandler : IRequestHandler<AuthenticateUserAccountCommand, bool>
    {
        private readonly UserManager<User> _userManager;

        public AuthenticateUserAccountHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(AuthenticateUserAccountCommand request, CancellationToken cancellationToken)
        {
            var isAuthModeEmail = AuthenticationModeChecker.IsEmail(request.UserField);
            
            if (isAuthModeEmail)
            {
                var user = await _userManager.FindByEmailAsync(request.UserField);
                if(user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return true;
                }
                return false;
            }
            else
            {
                var user = await _userManager.FindByNameAsync(request.UserField);
                if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
