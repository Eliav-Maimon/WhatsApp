namespace backend.Models
{
    public class Chat
    {
        public string Title { get; set; }
        public Message[] Messages { get; set; }
        public User[] Users { get; set; }
    }
}