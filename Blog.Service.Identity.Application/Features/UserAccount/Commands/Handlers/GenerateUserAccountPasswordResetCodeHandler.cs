using Blog.Service.Identity.Application.IntegrationEvents;
using Blog.Service.Identity.Domain.User;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedLibrary;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands.Handlers
{
    class GenerateUserAccountPasswordResetCodeHandler : IRequestHandler<GenerateUserAccountPasswordResetCodeCommand, bool>
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly UserManager<User> _userManager;

        public GenerateUserAccountPasswordResetCodeHandler(UserManager<User> userManager, IPublishEndpoint endpoint)
        {
            _userManager = userManager;
            _endpoint = endpoint;
        }

        public async Task<bool> Handle(GenerateUserAccountPasswordResetCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Trim());

            if (user == null)
            {
                throw new ApplicationException("User not found with that email Id");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            await NotificationEvent<GeneratePasswordResetCodeNotification>.Publish(_endpoint, new GeneratePasswordResetCodeNotification()
            {
                Title = "Account register confirmation",
                Message = "New account is registered successfully",
                Code = code
            });

            return true;
        }
    }
}
