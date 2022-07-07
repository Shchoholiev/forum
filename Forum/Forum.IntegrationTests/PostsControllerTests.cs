using Xunit;
using System.Net;
using Thread = Forum.Domain.Entities.Thread;
using Forum.Application.Models;
using System.Text;
using Newtonsoft.Json;
using Forum.Domain.Entities.Identity;
using Forum.Domain.Entities;

namespace Forum.IntegrationTests
{
    public class PostsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetPage_ReturnsPostsCollection()
        {
            var response = await this.HttpClient
                .GetAsync("api/posts?pageSize=10&pageNumber=1&threadId=62c42fbf4a055cc6a3591bcf");

            var collection = await response.Content.ReadAsAsync<IEnumerable<Post>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(collection);
        }

        [Fact]
        public async Task GetPage_ReturnsEmptyCollection()
        {
            var response = await this.HttpClient
                .GetAsync("api/posts?pageSize=10&pageNumber=2&threadId=62c42fbf4a055cc6a3591bcf");

            var collection = await response.Content.ReadAsAsync<IEnumerable<Post>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(collection);
        }

        [Fact]
        public async Task Create_NotAuthenticated_ReturnsUnauthorized()
        {
            var post = new Post
            {
                Text = "Integration Test",
                DatePosted = DateTime.Now,
                ThreadId = "62c42fbf4a055cc6a3591bcf"
            };
            var json = JsonConvert.SerializeObject(post);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await this.HttpClient.PostAsync("api/posts", stringContent);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Create_Authenticated_ReturnsCreatedAtActionPost()
        {
            await this.AuthenticateAsync("integration@test");
            var post = new Post
            {
                Text = "Integration Test",
                DatePosted = DateTime.Now,
                ThreadId = "62c42fbf4a055cc6a3591bcf"
            };
            var json = JsonConvert.SerializeObject(post);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await this.HttpClient.PostAsync("api/posts", stringContent);
            var responseContent = await response.Content.ReadAsAsync<Post>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.IsType<Post>(responseContent);
            Assert.NotNull(responseContent.Id);
        }

        [Fact]
        public async Task Update_Authenticated_ReturnsNoContent()
        {
            await this.AuthenticateAsync("integration@test");
            var user = new User
            {
                Id = "62c42fbf4a055cc6a3591bd0",
                Nickname = "IntegrationTest",
                Email = "integration@test",
                PasswordHash = "",
                Bio = "Tester",
                Reputation = 100,
            };
            var post = new Post
            {
                Id = "62c42fbf4a055cc6a3591bd1",
                Text = "Updated Post",
                DatePosted = DateTime.Now.AddDays(-1),
                Author = user
            };
            var json = JsonConvert.SerializeObject(post);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await this.HttpClient.PutAsync("api/posts/62c42fbf4a055cc6a3591bd1", stringContent);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Fact]
        public async Task Update_Authenticated_ReturnsBadRequestError()
        {
            await this.AuthenticateAsync("integration@test");
            var user = new User
            {
                Id = "62c42fbf4a055cc6a3591bd0",
                Nickname = "IntegrationTest",
                Email = "invalid@test",
                PasswordHash = "",
                Bio = "Tester",
                Reputation = 100,
            };
            var post = new Post
            {
                Id = "62c42fbf4a055cc6a3591bd5",
                Text = "Updated Post",
                DatePosted = DateTime.Now.AddDays(-1),
                Author = user
            };
            var json = JsonConvert.SerializeObject(post);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await this.HttpClient.PutAsync("api/posts/62c42fbf4a055cc6a3591bd5", stringContent);
            var error = await response.Content.ReadAsAsync<ErrorDetails>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsType<ErrorDetails>(error);
        }

        [Fact]
        public async Task Delete_Authenticated_ReturnsNoContent()
        {
            await this.AuthenticateAsync("integration@test2");

            var response = await this.HttpClient.DeleteAsync("api/posts/62c42fbf4a055cc6a3591bd5");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Authenticated_ReturnsNotFound()
        {
            await this.AuthenticateAsync("integration@test");

            var response = await this.HttpClient.DeleteAsync("api/posts/62c42fbf4a055cc6a3591be7");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Authenticated_ReturnsBadRequest()
        {
            await this.AuthenticateAsync("integration@test");

            var response = await this.HttpClient.DeleteAsync("api/posts/62c42fbf4a055cc6a3591bd5");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
