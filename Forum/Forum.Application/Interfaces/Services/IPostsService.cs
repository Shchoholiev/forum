using EducationalPortal.Application.Paging;
using Forum.Application.Paging;
using Forum.Domain.Entities;
using System.Linq.Expressions;

namespace Forum.Application.Interfaces.Services
{
    public interface IPostsService
    {
        Task AddAsync(Post post);

        Task UpdateAsync(Post post);

        Task DeleteAsync(string id);

        Task<PagedList<Post>> GetPageAsync(PageParameters pageParameters, string threadId);
    }
}
