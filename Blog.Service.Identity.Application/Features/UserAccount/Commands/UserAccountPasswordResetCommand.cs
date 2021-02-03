using MediatR;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands
{
    public class UserAccountPasswordResetCommand :  IRequest<bool>
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
