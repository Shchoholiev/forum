using EducationalPortal.Application.Paging;
using Forum.Application.Interfaces.Repositories;
using Forum.Application.Interfaces.Services;
using Forum.Application.Interfaces.Services.Identity;
using Forum.Application.Models;
using Forum.Domain.Entities.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Forum.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<User> _usersRepository;

        private readonly IUserManager _userManager;

        private readonly ILogger _logger;

        public AccountService(IGenericRepository<User> userRepository, IUserManager userManager,
                              ILogger<AccountService> logger)
        {
            this._usersRepository = userRepository;
            this._userManager = userManager;
            this._logger = logger;
        }

        public async Task<TokensModel> RegisterAsync(RegisterModel register)
        {
            var tokens = await this._userManager.RegisterAsync(register);
            
            this._logger.LogInformation($"Registered user with email: {register.Email}.");

            return tokens;
        }

        public async Task<TokensModel> LoginAsync(LoginModel login)
        {
            var tokens = await this._userManager.LoginAsync(login);

            this._logger.LogInformation($"Logged in user with email: {login.Email}.");

            return tokens;
        }

        public async Task<User> GetUserAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await this._usersRepository.GetOneAsync(filter);
        }

        public async Task<TokensModel> UpdateAsync(string email, User user)
        {
            return await this._userManager.UpdateAsync(email, user);
        }
    }
}
