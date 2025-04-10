using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Auth
{
    public class FogotPasswordCommand : IRequest<Result>
    {
        public string? Token { get; set; }
        public string? NewPassword {  get; set; }
    }
}
