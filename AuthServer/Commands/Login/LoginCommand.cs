using MediatR;

namespace AuthServer.Commands.Login
{
    public class LoginCommand : IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
