using Blog.Service.Identity.Domain.SeedWork;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Service.Identity.Domain.Role
{
    public class Role : IdentityRole<Guid>
    {
        [Required]
        public bool RecordStatus { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }
    }
}
