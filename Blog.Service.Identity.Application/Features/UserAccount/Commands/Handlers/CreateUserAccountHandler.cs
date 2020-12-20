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
            throw new System.NotImplementedException();
        }
    }
}
