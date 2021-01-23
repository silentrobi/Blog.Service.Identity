using System;

namespace SharedLibrary
{
    public class Notification
    {
        public DateTime Date => DateTime.Now;
        public Guid Id => Guid.NewGuid();
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
