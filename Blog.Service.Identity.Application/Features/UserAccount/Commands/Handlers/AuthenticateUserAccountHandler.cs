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
            bool result;

            if (isAuthModeEmail)
            {
                result = await AuthenticateUserAccountByEmail(request);
            }
            else
            {
                result = await AuthenticateUserAccountByUserName(request);
            }

            return result;
        }

        private async Task<bool> AuthenticateUserAccountByEmail(AuthenticateUserAccountCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.UserField);

            return user != null && await _userManager.CheckPasswordAsync(user, request.Password);
        }

        private async Task<bool> AuthenticateUserAccountByUserName(AuthenticateUserAccountCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.UserField);

            return user != null && await _userManager.CheckPasswordAsync(user, request.Password);
        }
    }
}
