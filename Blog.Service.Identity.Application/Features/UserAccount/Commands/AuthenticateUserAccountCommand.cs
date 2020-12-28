using MediatR;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands
{
    public class AuthenticateUserAccountCommand : IRequest<bool>
    {
        public string UserField { get; set; }
        public string Password { get; set; }
    }
}
