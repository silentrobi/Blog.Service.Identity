using Blog.Service.Identity.Domain.User;

namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterResponseViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public RegisterResponseViewModel(User user)
        {
            Id = user.Id.ToString();
            UserName = user.UserName;
            Email = user.Email;
        }
    }
}
