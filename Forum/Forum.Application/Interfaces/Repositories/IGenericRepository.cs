using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using Forum.Domain.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Forum.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(ObjectId id);

        Task<TEntity> GetOneAsync(ObjectId id);

        Task<TEntity> GetOneAsync(FilterDefinition<TEntity> filter);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, SortDefinition<TEntity> sort);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, FilterDefinition<TEntity> filter,
                                              SortDefinition<TEntity> sort);
    }
}
