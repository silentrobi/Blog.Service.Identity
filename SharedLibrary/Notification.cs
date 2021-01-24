using System;

namespace SharedLibrary
{
    public abstract class Notification
    {
        public DateTime Date => DateTime.Now;
        public Guid Id => Guid.NewGuid();
        public string Title { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
    }
}
