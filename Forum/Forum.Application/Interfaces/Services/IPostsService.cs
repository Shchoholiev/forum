using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using Forum.Domain.Entities;

namespace Forum.Application.Interfaces.Services
{
    public interface IPostsService
    {
        Task AddAsync(Post post);

        Task UpdateAsync(Post post, string userEmail);

        Task DeleteAsync(string id, string userEmail);

        Task<PagedList<Post>> GetPageAsync(PageParameters pageParameters, string threadId);
    }
}
