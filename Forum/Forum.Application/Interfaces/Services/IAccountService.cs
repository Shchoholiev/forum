using Forum.Application.Models;
using Forum.Domain.Entities.Identity;

namespace Forum.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<User> GetUserAsync(string email);

        Task<TokensModel> RegisterAsync(RegisterModel register);

        Task<TokensModel> LoginAsync(LoginModel login);

        Task<TokensModel> UpdateAsync(string email, User user);
    }
}
