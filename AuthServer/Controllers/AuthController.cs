using AuthServer.Models;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using AuthServer.Commands.Login;
using AuthServer.Commands.CreateUser;
using Microsoft.AspNetCore.Authorization;
using AuthServer.Commands.LogOut;

namespace AuthServer.Controllers
{
    [Controller]
    [Route("[controller]/[action]")]
    public class AuthController : BaseController
    {
        IMediator _mediator;

        public AuthController(IMediator mediator) =>
            _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel viewModel)
        {
            var command = new LoginCommand
            {
                UserName = viewModel.UserName,
                Password = viewModel.Password,
            };

            string token = await _mediator.Send(command);

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // TODO: Валидация DTO
            var command = new CreateUserCommand
            {
                UserName = registerDto.UserName,
                Password = registerDto.Password,
            };

            string token = await _mediator.Send(command);

            return Ok(token);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var command = new LogOutCommand
            {
                Id = UserId,
            };

            await _mediator.Send(command);

            return Ok();
        }
    }
}