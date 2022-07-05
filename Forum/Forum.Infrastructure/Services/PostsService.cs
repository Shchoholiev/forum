using EducationalPortal.Application.Paging;
using Forum.Application.Interfaces.Repositories;
using Forum.Application.Interfaces.Services;
using Forum.Application.Paging;
using Forum.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Forum.Infrastructure.Services
{
    public class PostsService : IPostsService
    {
        private readonly IGenericRepository<Post> _postsRepository;

        private readonly ILogger _logger;

        public PostsService(IGenericRepository<Post> postsRepository, ILogger<PostsService> logger)
        {
            this._postsRepository = postsRepository;
            this._logger = logger;
        }

        public async Task AddAsync(Post post)
        {
            await this._postsRepository.AddAsync(post);

            this._logger.LogInformation($"Added post with id: {post.Id}.");
        }

        public async Task DeleteAsync(string id)
        {
            await this._postsRepository.DeleteAsync(ObjectId.Parse(id));

            this._logger.LogInformation($"Deleted post with id: {id}.");
        }

        public async Task UpdateAsync(Post post)
        {
            await this._postsRepository.UpdateAsync(post);

            this._logger.LogInformation($"Updated post with id: {post.Id}.");
        }

        public async Task<PagedList<Post>> GetPageAsync(PageParameters pageParameters, string threadId)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.ThreadId, ObjectId.Parse(threadId));
            var sort = Builders<Post>.Sort.Descending(p => p.DatePosted);
            var posts = await this._postsRepository.GetPageAsync(pageParameters, filter, sort);

            this._logger.LogInformation($"Returned posts page {posts.PageNumber} from database.");

            return posts;
        }
    }
}
