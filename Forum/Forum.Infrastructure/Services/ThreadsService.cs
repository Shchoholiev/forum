using EducationalPortal.Application.Paging;
using Forum.Application.Exceptions;
using Forum.Application.Interfaces.Repositories;
using Forum.Application.Interfaces.Services;
using Forum.Application.Paging;
using Forum.Domain.Entities.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Thread = Forum.Domain.Entities.Thread;

namespace Forum.Infrastructure.Services
{
    public class ThreadsService : IThreadsService
    {
        private readonly IGenericRepository<Thread> _threadsRepository;

        private readonly IGenericRepository<User> _usersRepository;

        private readonly ILogger _logger;

        public ThreadsService(IGenericRepository<Thread> threadsRepository, 
                              IGenericRepository<User> usersRepository, ILogger<PostsService> logger)
        {
            this._threadsRepository = threadsRepository;
            this._usersRepository = usersRepository;
            this._logger = logger;
        }

        public async Task AddAsync(Thread thread, string authorEmail)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, authorEmail);
            var user = this._usersRepository.GetOneAsync(filter);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            await this._threadsRepository.AddAsync(thread);

            this._logger.LogInformation($"Added thread with id: {thread.Id}.");
        }

        public async Task UpdateAsync(Thread thread, string userEmail)
        {
            if (thread.Author.Email != userEmail)
            {
                throw new InvalidDataException("You are not an author of this thread!");
            }

            await this._threadsRepository.UpdateAsync(thread);

            this._logger.LogInformation($"Updated thread with id: {thread.Id}.");
        }

        public async Task DeleteAsync(string id, string userEmail)
        {
            var thread = await this._threadsRepository.GetOneAsync(id);
            if (thread == null)
            {
                throw new NotFoundException("Thread");
            }

            if (thread.Author.Email != userEmail)
            {
                throw new InvalidDataException("You are not an author of this thread!");
            }

            await this._threadsRepository.DeleteAsync(thread.Id);

            this._logger.LogInformation($"Deleted thread with id: {id}.");
        }

        public async Task<Thread> GetOneAsync(string id)
        {
            var thread = await this._threadsRepository.GetOneAsync(id);

            if (thread == null)
            {
                throw new NotFoundException("Thread");
            }

            this._logger.LogInformation($"Returned thread with id: {thread.Id} from database.");

            return thread;
        }

        public async Task<PagedList<Thread>> GetPageAsync(PageParameters pageParameters)
        {
            var sort = Builders<Thread>.Sort.Descending(p => p.DatePosted);
            var threads = await this._threadsRepository.GetPageAsync(pageParameters, sort);

            this._logger.LogInformation($"Returned threads page {threads.PageNumber} from database.");

            return threads;
        }
    }
}
