using AuthServer.Models;
using MediatR;

namespace AuthServer.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
