using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        [RegularExpression("^(?!0{3})[0-9]{9}$")]
        public string Phone { get; set; }
        public User[] Contacts { get; set; }
        public bool IsOnline { get; set; }
        public string Image { get; set; }
        public DateTime LastSeen { get; set; }
    }
}