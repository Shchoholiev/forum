using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Forum.Application.Interfaces.Services
{
    public interface IThreadsServices
    {
        Task AddAsync(Thread thread);

        Task UpdateAsync(Thread thread);

        Task DeleteAsync(ObjectId id);

        Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters);

        Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters,
                                             Expression<Func<Thread, bool>> predicate);
    }
}
