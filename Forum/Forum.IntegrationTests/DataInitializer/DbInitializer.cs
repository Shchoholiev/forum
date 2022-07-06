using Forum.Domain.Entities;
using Forum.Domain.Entities.Identity;
using Forum.Infrastructure.MongoDB;
using Forum.Infrastructure.Services.Identity;
using Microsoft.Extensions.Logging;
using Thread = Forum.Domain.Entities.Thread;

namespace Forum.IntegrationTests.DataInitializer
{
    public static class DbInitializer
    {
        public static void Initialize(MongoDbContext context, ILogger<PasswordHasher> logger)
        {
            context.DropDatabase();
            var db = context.Db;

            var usersCollection = db.GetCollection<User>("Users");

            var passwordHasher = new PasswordHasher(logger);
            var passwordHash = passwordHasher.Hash("integrationtest");

            var user = new User
            {
                Id = "62c42fbf4a055cc6a3591bd0",
                Nickname = "IntegrationTest",
                Email = "integration@test",
                PasswordHash = passwordHash,
                Bio = "Tester",
                Reputation = 100,
            };

            usersCollection.InsertOne(user);

            var threadsCollection = db.GetCollection<Thread>("Threads");

            var thread = new Thread
            {
                Id = "62c42fbf4a055cc6a3591bcf",
                Name = "What about testing?",
                Rating = 284,
                Author = user,
                DatePosted = DateTime.Now.AddDays(-10),
            };

            threadsCollection.InsertOne(thread);

            var postsCollection = db.GetCollection<Post>("Posts");

            var post1 = new Post
            {
                Id = "62c42fbf4a055cc6a3591bd1",
                Text = "Integration testing is cool",
                Author = user,
                Likes = 34,
                DatePosted = DateTime.Now.AddHours(-5),
                ThreadId = thread.Id,
                RepliedTo = null,
            };

            var post2 = new Post
            {
                Id = "62c42fbf4a055cc6a3591bd5",
                Text = "Unit testing is way better",
                Author = user,
                Likes = 42,
                DatePosted = DateTime.Now.AddHours(-5),
                ThreadId = thread.Id,
                RepliedTo = post1,
            };

            postsCollection.InsertMany(new List<Post> { post1, post2 });
        }
    }
}
