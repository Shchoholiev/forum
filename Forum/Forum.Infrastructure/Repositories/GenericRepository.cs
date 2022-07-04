using EducationalPortal.Application.Paging;
using Forum.Application.Helpers;
using Forum.Application.Interfaces.Repositories;
using Forum.Application.Paging;
using Forum.Domain.Common;
using Forum.Infrastructure.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Forum.Infrastructure.Repositories
{
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        public readonly IMongoDatabase _db;

        private readonly IMongoCollection<TEntity> _collection;

        public GenericRepository(MongoDbContext context)
        {
            this._db = context.Db;
            this._collection = this._db.GetCollection<TEntity>(Text.ToPlural(typeof(TEntity).Name));
        }

        public async Task AddAsync(TEntity entity)
        {
            await this._collection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            await this._collection.DeleteOneAsync(filter);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            await this._collection.ReplaceOneAsync(filter, entity);
        }

        public async Task<TEntity> GetOneAsync(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            return await (await this._collection.FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetOneAsync(FilterDefinition<TEntity> filter)
        {
            return await (await this._collection.FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters)
        {
            var emptyFilter = Builders<TEntity>.Filter.Empty;
            var entities = await this._collection.Find(emptyFilter)
                                                 .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                                 .Limit(pageParameters.PageSize)
                                                 .ToListAsync();
            var totalCount = (int) await this._collection.CountDocumentsAsync(emptyFilter);

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                                           FilterDefinition<TEntity> filter)
        {
            var entities = await this._collection.Find(filter)
                                                 .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                                 .Limit(pageParameters.PageSize)
                                                 .ToListAsync();
            var totalCount = (int)await this._collection.CountDocumentsAsync(filter);

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }
    }
}
