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
    public class ChangeUserAccountPasswordHandler : IRequestHandler<ChangeUserAccountPasswordCommand, bool>
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly UserManager<User> _userManager;

        public ChangeUserAccountPasswordHandler(UserManager<User> userManager, IPublishEndpoint endpoint)
        {
            _userManager = userManager;
            _endpoint = endpoint;
        }

        public async Task<bool> Handle(ChangeUserAccountPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Trim());

            if (user == null)
            {
                throw new ApplicationException("User not found with that email Id");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded) 
            {
                await NotificationEvent<ChangePasswordNotification>.Publish(_endpoint, new ChangePasswordNotification()
                {
                    Title = "Account password change confirmation",
                    Message = "Account password has changed successfully"
                });

                return true;
            }

            return false;
        }
    }
}
