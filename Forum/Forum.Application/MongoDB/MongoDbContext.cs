using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Forum.Application.MongoDB
{
    public class MongoDbContext
    {
        private readonly MongoClient _client;

        private readonly IMongoDatabase _db;

        public MongoDbContext(IConfiguration configuration)
        {
            this._client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            this._db = this._client.GetDatabase(configuration.GetValue<string>("MongoDatabaseName"));
        }

        public IMongoDatabase Db { get => this._db; }
    }
}
