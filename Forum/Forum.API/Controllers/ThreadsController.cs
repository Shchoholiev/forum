using Forum.Application.Interfaces.Services;
using Forum.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thread = Forum.Domain.Entities.Thread;

namespace Forum.API.Controllers
{
    public class ThreadsController : ApiControllerBase
    {
        private readonly IThreadsService _threadsService;

        public ThreadsController(IThreadsService threadsService)
        {
            this._threadsService = threadsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thread>>> GetPage([FromQuery] PageParameters pageParameters)
        {
            var threads = await this._threadsService.GetPageAsync(pageParameters);
            this.SetPagingMetadata(threads);
            return threads;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Thread>> GetThread(string id)
        {
            return await this._threadsService.GetOneAsync(id);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Thread thread)
        {
            await this._threadsService.AddAsync(thread);
            return CreatedAtAction("GetThread", new { id = thread.Id }, thread);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Thread thread)
        {
            await this._threadsService.UpdateAsync(thread, Email);
            return NoContent();
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await this._threadsService.DeleteAsync(id, Email);
            return NoContent();
        }
    }
}
