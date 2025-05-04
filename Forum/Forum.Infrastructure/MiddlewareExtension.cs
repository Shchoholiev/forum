using Forum.Application.Interfaces.Repositories;
using Forum.Application.Interfaces.Services;
using Forum.Application.Interfaces.Services.Identity;
using Forum.Infrastructure.ExceptionHandling;
using Forum.Infrastructure.MongoDB;
using Forum.Infrastructure.Repositories;
using Forum.Infrastructure.Services;
using Forum.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Forum.Infrastructure
{
    public static class MiddlewareExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<IThreadsService, ThreadsService>();

            return services;
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static ILoggingBuilder AddLogger(this ILoggingBuilder logging, IConfiguration configuration)
        {
            logging.ClearProviders();
            logging.AddNLog(configuration);

            return logging;
        }
    }
}
