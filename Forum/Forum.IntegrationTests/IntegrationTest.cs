using Forum.Application.Models;
using Forum.Infrastructure.MongoDB;
using Forum.Infrastructure.Services.Identity;
using Forum.IntegrationTests.DataInitializer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Forum.IntegrationTests
{
    public class IntegrationTest
    {
        private readonly HttpClient _client;

        public HttpClient HttpClient => _client;

        public IntegrationTest()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                    config.AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.Integration.json")
                );
            });
            this._client = factory.CreateDefaultClient();

            var mongoContext = factory.Services.GetService<MongoDbContext>();
            var logger = factory.Services.GetService<ILogger<PasswordHasher>>();
            DbInitializer.Initialize(mongoContext, logger);
        }

        public async Task AuthenticateAsync(string email)
        {
            this._client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", await GetJwtAsync(email));
        }

        private async Task<string> GetJwtAsync(string email)
        {
            var loginModel = new LoginModel 
            { 
                Email = email,
                Password = "integrationtest"
            };
            var json = JsonConvert.SerializeObject(loginModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await this._client.PostAsync("api/account/login", stringContent);
            var tokens = await response.Content.ReadAsAsync<TokensModel>();

            return tokens.AccessToken;
        }
    }
}
