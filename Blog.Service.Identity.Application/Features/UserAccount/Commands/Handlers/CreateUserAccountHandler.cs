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
    public class CreateUserAccountHandler : IRequestHandler<CreateUserAccountCommand, User>
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly UserManager<User> _userManager;

        public CreateUserAccountHandler(UserManager<User> userManager, IPublishEndpoint endpoint)
        {
            _userManager = userManager;
            _endpoint = endpoint;
        }

        public async Task<User> Handle(CreateUserAccountCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new Exceptions.ApplicationException(result.Errors.ToString());

            // Add some user claims. Note these claims get embedded in the tokens returned to the client during the authentication phase.

            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", "Consumer"));

            //await NotificationEvent<AccountCreateNotification>.Raise(_endpoint, new AccountCreateNotification()
            //{
            //    Title = "Account register confirmation",
            //    Message = "New account is registered successfully",
            //    Email = request.Email
            //});

            return user;
        }
    }
}
