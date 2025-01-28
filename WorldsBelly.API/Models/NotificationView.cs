using System;
using System.Collections.Generic;

namespace WorldsBelly.API.Models
{
    public class NotificationView
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ReadAt { get; set; }
        public bool IsRead { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Action { get; set; }
    }
}
