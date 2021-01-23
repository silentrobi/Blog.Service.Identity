using Blog.Service.Identity.Application.Exceptions;
using Blog.Service.Identity.Application.IntegrationEvents;
using Blog.Service.Identity.Domain.User;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedLibrary;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands.Handlers
{
    public class UserAccountPasswordResetHandler : IRequestHandler<UserAccountPasswordResetCommand, bool>
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly UserManager<User> _userManager;

        public UserAccountPasswordResetHandler(UserManager<User> userManager, IPublishEndpoint endpoint)
        {
            _userManager = userManager;
            _endpoint = endpoint;
        }

        public async Task<bool> Handle(UserAccountPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Trim());

            if (user == null)
            {
                throw new ApplicationException("User not found with that email Id");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Code.Trim(), request.NewPassword);

            if (result.Succeeded)
            {
                await NotificationEvent<AccountPasswordResetNotification>.Raise(_endpoint, new AccountPasswordResetNotification());
            }

            return result.Succeeded;
        }
    }
}
