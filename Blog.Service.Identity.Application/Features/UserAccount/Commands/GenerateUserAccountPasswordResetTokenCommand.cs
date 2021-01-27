using MediatR;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands
{
    public class GenerateUserAccountPasswordResetTokenCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }
}
