using Forum.Application.Interfaces.Services;
using Forum.Application.Paging;
using Forum.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    public class PostsController : ApiControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            this._postsService = postsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPage([FromQuery] PageParameters pageParameters, 
                                                                   string threadId)
        {
            var posts = await this._postsService.GetPageAsync(pageParameters, threadId);
            this.SetPagingMetadata(posts);
            return posts;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            await this._postsService.AddAsync(post);
            return StatusCode(201);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Post post)
        {
            await this._postsService.UpdateAsync(post, Email);
            return NoContent();
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await this._postsService.DeleteAsync(id, Email);
            return NoContent();
        }
    }
}
