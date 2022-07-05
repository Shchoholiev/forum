using Forum.API.Controllers;
using Forum.Application.Interfaces.Services;
using Forum.Application.Models;
using Forum.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<User>> Profile()
        {
            return await this._accountService.GetUserAsync(Email);
        }

        [HttpPut]
        public async Task<ActionResult<TokensModel>> Update([FromBody] User user)
        {
            return await this._accountService.UpdateAsync(Email, user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<TokensModel>> Register([FromBody] RegisterModel model)
        {
            return await this._accountService.RegisterAsync(model);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokensModel>> Login([FromBody] LoginModel model)
        {
            return await this._accountService.LoginAsync(model);
        }
    }
}
