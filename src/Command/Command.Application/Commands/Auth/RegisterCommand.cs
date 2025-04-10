using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Auth
{
    public class RegisterCommand : IRequest<Result>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
