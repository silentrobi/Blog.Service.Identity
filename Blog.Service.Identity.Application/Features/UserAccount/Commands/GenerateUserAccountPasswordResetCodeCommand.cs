using MediatR;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands
{
    class GenerateUserAccountPasswordResetCodeCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }
}
