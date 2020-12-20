using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Service.Identity.Domain.User
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public bool RecordStatus { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }
        public  bool TwoFactorEnabled { get; set; }
        public  bool PhoneNumberConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public  string PasswordHash { get; set; }
        public  bool EmailConfirmed { get; set; }
        public  string Email { get; set; }
        public  string UserName { get; set; }
        public string Name { get; set; }
        public  int AccessFailedCount { get; set; }
    }

}
