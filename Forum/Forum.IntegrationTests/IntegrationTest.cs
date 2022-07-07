using Forum.Application.Models;
using Forum.Infrastructure.MongoDB;
using Forum.Infrastructure.Services.Identity;
using Forum.IntegrationTests.DataInitializer;
using Microsoft.AspNetCore.Mvc.Testing;
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
            });
            this._client = factory.CreateDefaultClient();

            var mongoContext = factory.Services.GetService<MongoDbContext>();
            var logger = factory.Services.GetService<ILogger<PasswordHasher>>();
            DbInitializer.Initialize(mongoContext, logger);
        }

        public async Task AuthenticateAsync()
        {
            this._client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var loginModel = new LoginModel 
            { 
                Email = "integration@test",
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
