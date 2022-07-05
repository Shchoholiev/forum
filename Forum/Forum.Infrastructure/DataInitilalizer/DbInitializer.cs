using Forum.Domain.Entities;
using Forum.Domain.Entities.Identity;
using Forum.Infrastructure.MongoDB;
using Forum.Infrastructure.Services.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Thread = Forum.Domain.Entities.Thread;

namespace Forum.Infrastructure.DataInitilalizer
{
    public static class DbInitializer
    {
        public static async Task Initialize(MongoDbContext context, ILogger<PasswordHasher> logger)
        {
            var db = context.Db;

            #region Users

            var usersCollection = db.GetCollection<User>("Users");

            var passwordHasher = new PasswordHasher(logger);
            var passwordHash = passwordHasher.Hash("12345Yuiop-");

            var rick = new User
            {
                Nickname = "Rick",
                Email = "rick@gmail.com",
                PasswordHash = passwordHash,
                Bio = "Unity dev",
                Reputation = 100,
            };

            var joe = new User
            {
                Nickname = "Joe18",
                Email = "joe@gmail.com",
                PasswordHash = passwordHash,
                Bio = "Gamer from LA",
                Reputation = 12,
            };

            var jason = new User
            {
                Nickname = "JasonGallery",
                Email = "jason@gmail.com",
                PasswordHash = passwordHash,
                Bio = "2D Artist",
                Reputation = 12,
            };

            await usersCollection.InsertManyAsync(new List<User> { rick, joe, jason });

            #endregion

            #region Threads

            var threadsCollection = db.GetCollection<Thread>("Threads");

            var multiplayer = new Thread
            {
                Name = "Multiplayer features",
                Rating = 356,
                Author = rick,
                DatePosted = DateTime.Now.AddDays(-10),
            };

            var bestBook = new Thread
            {
                Name = "Best book for Unity begginers",
                Rating = 356,
                Author = rick,
                DatePosted = DateTime.Now.AddDays(-13),
            };

            await threadsCollection.InsertManyAsync(new List<Thread> { multiplayer, bestBook });

            #endregion

            #region Posts

            var postsCollection = db.GetCollection<Post>("Posts");

            var random = new Random();
            for (int i = 100; i >= 0; i--)
            {
                var post = new Post
                {
                    Text = LoremIpsum.GenerateText(),
                    Author = random.Next(1, 3) switch
                    {
                        1 => rick,
                        2 => joe,
                        3 => jason,
                    },
                    Likes = random.Next(0, 50),
                    DatePosted = DateTime.Now.AddHours(-i),
                    ThreadId = multiplayer.Id,
                    RepliedTo = (random.Next(1,2) == 1) 
                                ? await postsCollection.AsQueryable().Sample(1).FirstOrDefaultAsync()
                                : null,
                };

                await postsCollection.InsertOneAsync(post);
            }

            for (int i = 100; i >= 0; i--)
            {
                var post = new Post
                {
                    Text = LoremIpsum.GenerateText(),
                    Author = random.Next(1, 3) switch
                    {
                        1 => rick,
                        2 => joe,
                        3 => jason,
                    },
                    Likes = random.Next(0, 50),
                    DatePosted = DateTime.Now.AddHours(-i),
                    ThreadId = bestBook.Id,
                    RepliedTo = (random.Next(1, 2) == 1)
                                ? await postsCollection.AsQueryable().Where(p => p.ThreadId == bestBook.Id)
                                                       .Sample(1).FirstOrDefaultAsync()
                                : null,
                };

                await postsCollection.InsertOneAsync(post);
            }

            #endregion
        }
    }

    public static class LoremIpsum
    {
        private static readonly string _text = "Lorem ipsum dolor sit amet, consectetur adipiscing " +
            "elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad mi" +
            "nim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo co" +
            "nsequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore " +
            "eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culp" +
            "a qui officia deserunt mollit anim id est laborum. ";

        private static readonly Random _random = new();

        public static string GenerateText()
        {
            var length = _random.Next(10, 445);
            return _text.Substring(0, length);
        }
    }
}
