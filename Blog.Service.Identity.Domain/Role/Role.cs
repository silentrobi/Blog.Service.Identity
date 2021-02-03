using Microsoft.AspNetCore.Identity;
using System;

namespace Blog.Service.Identity.Domain.Role
{
    public class Role : IdentityRole<Guid>
    {
        public bool? RecordStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
