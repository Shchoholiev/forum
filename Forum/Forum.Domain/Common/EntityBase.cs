using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Forum.Domain.Common
{
    public abstract class EntityBase
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
