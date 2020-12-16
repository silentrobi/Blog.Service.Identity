using Blog.Service.Identity.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace Blog.Service.Identity.Domain.IdentityUser
{
    public class IdentityUser : BaseEntity
    {
        public  bool TwoFactorEnabled { get; set; }
        public  bool PhoneNumberConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public  string PasswordHash { get; set; }
        public  bool EmailConfirmed { get; set; }
        [Required]
        public  string Email { get; set; }
        [Required]
        public  string UserName { get; set; }
        [Required]
        public string Name { get; set; }
        public  int AccessFailedCount { get; set; }
    }

}
