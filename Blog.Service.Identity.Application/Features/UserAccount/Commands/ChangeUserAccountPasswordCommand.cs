using MediatR;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands
{
    public class ChangeUserAccountPasswordCommand : IRequest<bool>
    {
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }

        public string Email { get; set; }
    }
}
