using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using System.Linq.Expressions;
using Thread = Forum.Domain.Entities.Thread;

namespace Forum.Application.Interfaces.Services
{
    public interface IThreadsService
    {
        Task AddAsync(Thread thread);

        Task UpdateAsync(Thread thread);

        Task DeleteAsync(string id);

        Task<Thread> GetOneAsync(string id);

        Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters);
    }
}
