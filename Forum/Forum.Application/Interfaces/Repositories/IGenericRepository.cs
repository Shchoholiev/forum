using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using Forum.Domain.Common;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Forum.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(ObjectId id);

        Task<TEntity> GetOneAsync(ObjectId id);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, 
                                              Expression<Func<TEntity, bool>> predicate);
    }
}
