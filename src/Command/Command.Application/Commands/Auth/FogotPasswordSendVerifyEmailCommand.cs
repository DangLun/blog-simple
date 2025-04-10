using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Auth
{
    public class FogotPasswordSendVerifyEmailCommand : IRequest<Result>
    {
        public string? Email { get; set; }
    }
}
