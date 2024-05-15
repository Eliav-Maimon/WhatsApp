namespace backend.Models
{
    public class Message
    {
        public string SenderId { get; set; } // User id
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public bool IsSeen { get; set; }
    }
}