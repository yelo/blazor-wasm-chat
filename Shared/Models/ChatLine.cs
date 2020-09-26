using System;

namespace RetroChat.Shared.Models
{
    public class ChatLine
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; } = DateTime.Now;
        public User From { get; set; }
    }
}