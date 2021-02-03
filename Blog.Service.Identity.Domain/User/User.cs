using Blog.Service.Identity.Domain.SeedWork;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
namespace Blog.Service.Identity.Domain.User
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public bool? RecordStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
