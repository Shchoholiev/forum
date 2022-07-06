using Forum.Application.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Forum.IntegrationTests
{
    public class IntegrationTest
    {
        private readonly HttpClient _client;

        public IntegrationTest()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {

            });
            this._client = factory.CreateDefaultClient();
        }

        public async Task AuthenticateAsync()
        {
            this._client.DefaultRequestHeaders.Authorization = 
                AuthenticationHeaderValue.Parse(await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var registerModel = new RegisterModel 
            { 
                Nickname = "IntegrationTest",
                Email = "integration@test",
                Password = "integrationtest"
            };
            var json = JsonConvert.SerializeObject(registerModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await this._client.PostAsync("api/account/register", stringContent);
            var tokens = await response.Content.ReadAsAsync<TokensModel>();

            return tokens.AccessToken;
        }
    }
}
