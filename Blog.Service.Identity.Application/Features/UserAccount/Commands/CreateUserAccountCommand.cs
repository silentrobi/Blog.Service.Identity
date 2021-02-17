using Blog.Service.Identity.Domain.User;
using MediatR;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands
{
    public class CreateUserAccountCommand : IRequest<User>
    {
        public string PhoneNumber { get; set; }

        public  string Password { get; set; }
    
        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
