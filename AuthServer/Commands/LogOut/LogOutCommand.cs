using MediatR;

namespace AuthServer.Commands.LogOut
{
    public class LogOutCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
