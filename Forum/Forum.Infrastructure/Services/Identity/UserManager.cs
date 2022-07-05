using Forum.Application.Exceptions;
using Forum.Application.Interfaces.Repositories;
using Forum.Application.Interfaces.Services.Identity;
using Forum.Application.Models;
using Forum.Domain.Entities.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Security.Claims;

namespace Forum.Infrastructure.Services.Identity
{
    public class UserManager : IUserManager
    {
        private readonly IGenericRepository<User> _usersRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly ITokensService _tokensService;

        private readonly ILogger _logger;

        public UserManager(IGenericRepository<User> usersRepository, IPasswordHasher passwordHasher,
                           ITokensService tokensService, ILogger<UserManager> logger)
        {
            this._usersRepository = usersRepository;
            this._passwordHasher = passwordHasher;
            this._tokensService = tokensService;
            this._logger = logger;
        }

        public async Task<TokensModel> RegisterAsync(RegisterModel register)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, register.Email);
            if (await this._usersRepository.GetOneAsync(filter) != null)
            {
                throw new AlreadyExistsException("user email", register.Email);
            }

            var user = new User
            {
                Nickname = register.Nickname,
                Email = register.Email,
                PasswordHash = this._passwordHasher.Hash(register.Password),
                RefreshToken = this.GetRefreshToken(),
            };
            
            await this._usersRepository.AddAsync(user);
            var tokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Created user with email: {user.Email}.");

            return tokens;
        }

        public async Task<TokensModel> LoginAsync(LoginModel login)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, login.Email);
            var user = await this._usersRepository.GetOneAsync(filter);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (!this._passwordHasher.Check(login.Password, user.PasswordHash))
            {
                throw new InvalidDataException("Invalid password!");
            }

            user.RefreshToken = this.GetRefreshToken();
            await this._usersRepository.UpdateAsync(user);
            var tokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Logged in user with email: {login.Email}.");

            return tokens;
        }

        public async Task<TokensModel> UpdateAsync(string email, User newUser)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            var user = await this._usersRepository.GetOneAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var newEmailFilter = Builders<User>.Filter.Eq(u => u.Email, newUser.Email);
            if (email != newUser.Email && await this._usersRepository.GetOneAsync(newEmailFilter) != null)
            {
                throw new AlreadyExistsException("email", newUser.Email);
            }

            user.RefreshToken = this.GetRefreshToken();
            await this._usersRepository.UpdateAsync(user);
            var tokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Update user with email: {email}.");

            return tokens;
        }

        private RefreshToken GetRefreshToken()
        {
            var refreshToken = this._tokensService.GenerateRefreshToken();
            var token = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.Now.AddDays(7),
            };

            this._logger.LogInformation($"Returned new refresh token.");

            return token;
        }

        private TokensModel GetUserTokens(User user)
        {
            var claims = this.GetClaims(user);
            var accessToken = this._tokensService.GenerateAccessToken(claims);

            this._logger.LogInformation($"Returned new access and refresh tokens.");

            return new TokensModel
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken.Token
            };
        }

        private IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nickname),
                new Claim(ClaimTypes.Email, user.Email),
            };

            this._logger.LogInformation($"Returned claims for user with email: {user.Email}.");

            return claims;
        }
    }
}
