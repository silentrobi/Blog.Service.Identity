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
    public class GenerateUserAccountPasswordResetTokenHandler : IRequestHandler<GenerateUserAccountPasswordResetTokenCommand, bool>
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly UserManager<User> _userManager;

        public GenerateUserAccountPasswordResetTokenHandler(UserManager<User> userManager, IPublishEndpoint endpoint)
        {
            _userManager = userManager;
            _endpoint = endpoint;
        }

        public async Task<bool> Handle(GenerateUserAccountPasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Trim());

            if (user == null)
            {
                throw new ApplicationException("User not found with that email Id");
            }
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            await NotificationEvent<GeneratePasswordResetTokenNotification>.Raise(_endpoint, new GeneratePasswordResetTokenNotification()
            {
                Title = "Password recovery",
                Message = "Your password recovery token:",
                Token = token,
                Email = request.Email
            });

            return true;
        }
    }
}
