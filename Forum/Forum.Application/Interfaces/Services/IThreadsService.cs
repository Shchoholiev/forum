using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using Thread = Forum.Domain.Entities.Thread;

namespace Forum.Application.Interfaces.Services
{
    public interface IThreadsService
    {
        Task AddAsync(Thread thread, string authorEmail);

        Task UpdateAsync(Thread thread, string userEmail);

        Task DeleteAsync(string id, string userEmail);

        Task<Thread> GetOneAsync(string id);

        Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters);
    }
}
