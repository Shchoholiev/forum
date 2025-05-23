﻿using Forum.Application.Interfaces.Repositories;
using Forum.Application.Interfaces.Services.Identity;
using Forum.Application.Models;
using Forum.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Forum.Infrastructure.Services.Identity
{
    public class TokensService : ITokensService
    {
        private readonly IConfiguration _configuration;

        private readonly IGenericRepository<User> _usersRepository;

        private readonly ILogger _logger;

        public TokensService(IConfiguration configuration, IGenericRepository<User> usersRepository,
                             ILogger<TokensService> logger)
        {
            this._configuration = configuration;
            this._usersRepository = usersRepository;
            this._logger = logger;
        }

        public async Task<TokensModel> Refresh(TokensModel tokensModel, string email)
        {
            var principal = this.GetPrincipalFromExpiredToken(tokensModel.AccessToken);

            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            var user = await this._usersRepository.GetOneAsync(filter);
            if (user == null || user?.RefreshToken.Token != tokensModel.RefreshToken
                             || user?.RefreshToken.ExpiryDate <= DateTime.Now)
            {
                throw new SecurityTokenExpiredException();
            }

            var newAccessToken = this.GenerateAccessToken(principal.Claims);
            var newRefreshToken = this.GenerateRefreshToken();
            user.RefreshToken.Token = newRefreshToken;
            await this._usersRepository.UpdateAsync(user);

            this._logger.LogInformation($"Refreshed user tokens.");

            return new TokensModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }


        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokenOptions = GetTokenOptions(claims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            this._logger.LogInformation($"Generated new access token.");

            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);

                this._logger.LogInformation($"Generated new refresh token.");

                return refreshToken;
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    this._configuration.GetValue<string>("JsonWebTokenKeys:IssuerSigningKey"))),
                ValidateLifetime = false 
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                                            StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            this._logger.LogInformation($"Returned data from expired access token.");

            return principal;
        }

        private JwtSecurityToken GetTokenOptions(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                this._configuration.GetValue<string>("JsonWebTokenKeys:IssuerSigningKey")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: this._configuration.GetValue<string>("JsonWebTokenKeys:ValidIssuer"),
                audience: this._configuration.GetValue<string>("JsonWebTokenKeys:ValidAudience"),
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                signingCredentials: signinCredentials
            );

            return tokenOptions;
        }
    }
}
