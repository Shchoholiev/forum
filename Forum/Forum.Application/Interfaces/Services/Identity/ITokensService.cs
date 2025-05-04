using Forum.Application.Models;
using System.Security.Claims;

namespace Forum.Application.Interfaces.Services.Identity
{
    public interface ITokensService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        Task<TokensModel> Refresh(TokensModel tokens, string email);
    }
}
