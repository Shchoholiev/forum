using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Forum.Application.Interfaces.Services
{
    public interface IPostsService
    {
        Task AddAsync(Thread thread);

        Task UpdateAsync(Thread thread);

        Task DeleteAsync(string id);

        Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters, string threadId);

        Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters, string threadId,
                                             Expression<Func<Thread, bool>> predicate);
    }
}
