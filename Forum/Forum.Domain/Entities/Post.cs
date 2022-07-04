using Forum.Domain.Common;
using Forum.Domain.Entities.Identity;
using MongoDB.Bson;

namespace Forum.Domain.Entities
{
    public class Post : EntityBase
    {
        public ObjectId ThreadId { get; set; }

        public string Text { get; set; }

        public DateTime DatePosted { get; set; }

        public int Likes { get; set; }

        public User Author { get; set; }

        public Post RepliedTo { get; set; }
    }
}
