using Forum.Domain.Common;
using Forum.Domain.Entities.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Forum.Domain.Entities
{
    public class Post : EntityBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ThreadId { get; set; }

        public string Text { get; set; }

        public DateTime DatePosted { get; set; }

        public int Likes { get; set; } = 0;

        public User Author { get; set; }

        public Post? RepliedTo { get; set; }
    }
}
