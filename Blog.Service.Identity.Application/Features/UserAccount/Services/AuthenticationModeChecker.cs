namespace Blog.Service.Identity.Application.Features.UserAccount.Services
{
    public class AuthenticationModeChecker
    {
        public  static bool IsEmail(string value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(value);
                return addr.Address == value;
            }
            catch
            {
                return false;
            }
        }
    }
}
