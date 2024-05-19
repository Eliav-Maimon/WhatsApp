using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class User : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        // [JsonIgnore]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        // [RegularExpression("^0[23489][0-9]{7}$")]
        [RegularExpression("^[0-9]+$")]
        public string Phone { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Contacts { get; set; } 
        public bool IsOnline { get; set; }
        public string Image { get; set; }
        public DateTime LastSeen { get; set; }
    }
}