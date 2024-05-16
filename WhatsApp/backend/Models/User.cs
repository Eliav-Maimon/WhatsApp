using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [RegularExpression("^(?!0{3})[0-9]{9}$")]
        public long Phone { get; set; }
        // public User[] Contacts { get; set; }
        public Dictionary<string, User> Contacts { get; set; }
        public bool IsOnline { get; set; }
        public string Image { get; set; }
        public DateTime LastSeen { get; set; }
    }
}