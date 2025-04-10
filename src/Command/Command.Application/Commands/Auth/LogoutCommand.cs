using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Auth
{
    public class LogoutCommand : IRequest<Result>
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
