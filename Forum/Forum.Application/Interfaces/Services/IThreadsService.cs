using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Forum.Application.Interfaces.Services
{
    public interface IThreadsService
    {
        Task AddAsync(Thread thread);

        Task UpdateAsync(Thread thread);

        Task DeleteAsync(string id);

        Task<Thread> GetOneAsync(string id);

        Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters);

        Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters,
                                             Expression<Func<Thread, bool>> predicate);
    }
}
