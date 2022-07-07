using Xunit;
using System.Net;
using Thread = Forum.Domain.Entities.Thread;
using Forum.Application.Models;
using System.Text;
using Newtonsoft.Json;

namespace Forum.IntegrationTests
{
    public class ThreadsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetPage_ReturnsThreadsCollection()
        {
            var response = await this.HttpClient.GetAsync("api/threads?pageSize=10&pageNumber=1");

            var collection = await response.Content.ReadAsAsync<IEnumerable<Thread>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(collection);
        }

        [Fact]
        public async Task GetPage_ReturnsEmptyCollection()
        {
            var response = await this.HttpClient.GetAsync("api/threads?pageSize=10&pageNumber=2");

            var collection = await response.Content.ReadAsAsync<IEnumerable<Thread>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(collection);
        }

        [Fact]
        public async Task GetThread_ReturnsThread()
        {
            var response = await this.HttpClient.GetAsync("api/threads/62c42fbf4a055cc6a3591bcf");

            var thread = await response.Content.ReadAsAsync<Thread>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<Thread>(thread);
            Assert.Equal("62c42fbf4a055cc6a3591bcf", thread.Id);
        }

        [Fact]
        public async Task GetThread_ReturnsNotFoundError()
        {
            var response = await this.HttpClient.GetAsync("api/threads/62c42fbf4a055cc6a3591bd1");

            var error = await response.Content.ReadAsAsync<ErrorDetails>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsType<ErrorDetails>(error);
        }

        [Fact]
        public async Task Create_NotAuthenticated_ReturnsUnauthorized()
        {
            var thread = new Thread
            {
                Name = "Integration Test",
                DatePosted = DateTime.Now,
                Rating = 100,
            };
            var json = JsonConvert.SerializeObject(thread);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await this.HttpClient.PostAsync("api/threads", stringContent);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Create_Authenticated_ReturnsCreatedAtActionThread()
        {
            await this.AuthenticateAsync();
            var thread = new Thread
            {
                Name = "Integration Test",
                DatePosted = DateTime.Now,
                Rating = 100,
            };
            var json = JsonConvert.SerializeObject(thread);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await this.HttpClient.PostAsync("api/threads", stringContent);
            var responseContent = await response.Content.ReadAsAsync<Thread>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.IsType<Thread>(responseContent);
            Assert.NotNull(responseContent.Id);
        }


    }
}
