using System;

namespace Blog.Service.Identity.Domain.SeedWork
{
    public interface IBaseEntity
    {
        bool? RecordStatus { get; set; }
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
