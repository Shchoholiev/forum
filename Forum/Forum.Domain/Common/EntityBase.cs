using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Forum.Domain.Common
{
    public abstract class EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
